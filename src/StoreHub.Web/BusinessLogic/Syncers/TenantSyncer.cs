using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StoreHub.Web.BusinessLogic.Factories;
using StoreHub.Web.Core.Extensions;
using StoreHub.Web.Core.Models;
using StoreHub.Web.Core.Models.CompositeModels;
using StoreHub.Web.DataAccess;

namespace StoreHub.Web.BusinessLogic.Syncers
{
    public interface ITenantSyncer
    {
        Task<Dictionary<string, TenantModel>> ProcessTenants(SyncJobCompositeModel syncJobCompositeModel, SpaceModel space, CancellationToken stoppingToken);
    }

    public class TenantSyncer(
        ILogger<TenantSyncer> logger,        
        IOctopusRepository octopusRepository,
        IStoreHubDataAdapter storeHubDataAdapter,
        ISyncLogModelFactory syncLogModelFactory) : ITenantSyncer
    {
        public async Task<Dictionary<string, TenantModel>> ProcessTenants(SyncJobCompositeModel syncJobCompositeModel, SpaceModel space, CancellationToken stoppingToken)
        {
            await LogInformation($"Getting all the tenants for {syncJobCompositeModel.InstanceModel.Name}:{space.Name}", syncJobCompositeModel);
            var octopusList = await octopusRepository.GetAllTenantsForSpaceAsync(syncJobCompositeModel.InstanceModel, space);
            await LogInformation($"{octopusList.Count} tenants(s) found in {syncJobCompositeModel.InstanceModel.Name}:{space.Name}", syncJobCompositeModel);

            var returnObject = new Dictionary<string, TenantModel>();
            foreach (var item in octopusList)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }

                await LogInformation($"Checking to see if tenant {item.OctopusId}:{item.Name} already exists", syncJobCompositeModel);
                var itemModel = await storeHubDataAdapter.GetByOctopusIdAsync<TenantModel>(item.OctopusId);
                await LogInformation($"{(itemModel != null ? "Tenant already exists, updating" : "Unable to find tenant, creating")}", syncJobCompositeModel);
                item.Id = itemModel?.Id ?? 0;

                await LogInformation($"Saving tenant {item.OctopusId}:{item.Name} to the database", syncJobCompositeModel);
                var modelToTrack = item.Id > 0 ? await storeHubDataAdapter.UpdateAsync(item) : await storeHubDataAdapter.InsertAsync(item);

                await LogInformation($"Adding tenant {item.OctopusId}:{item.Name} to our sync dictionary for faster lookup", syncJobCompositeModel);
                returnObject.Add(item.OctopusId, modelToTrack);
            }

            return returnObject;
        }       

        private async Task LogInformation(string message, SyncJobCompositeModel syncJobCompositeModel)
        {
            var formattedMessage = $"{syncJobCompositeModel.GetMessagePrefix()}{message}";
            logger.LogInformation(formattedMessage);
            await storeHubDataAdapter.InsertAsync(syncLogModelFactory.MakeInformationLog(formattedMessage, syncJobCompositeModel.SyncModel.Id));
        }
    }
}
