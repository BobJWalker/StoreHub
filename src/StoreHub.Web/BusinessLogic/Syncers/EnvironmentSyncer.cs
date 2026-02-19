using System.Collections.Generic;
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
    public interface IEnvironmentSyncer
    {
        Task<Dictionary<string, EnvironmentModel>> ProcessEnvironments(SyncJobCompositeModel syncJobCompositeModel, SpaceModel space, CancellationToken stoppingToken);
    }

    public class EnvironmentSyncer (
        ILogger<EnvironmentSyncer> logger,        
        IOctopusRepository octopusRepository,
        IStoreHubDataAdapter storeHubDataAdapter,
        ISyncLogModelFactory syncLogModelFactory) : IEnvironmentSyncer
    {        
        public async Task<Dictionary<string, EnvironmentModel>> ProcessEnvironments(SyncJobCompositeModel syncJobCompositeModel, SpaceModel space, CancellationToken stoppingToken)
        {
            await LogInformation($"Getting all the environments for {syncJobCompositeModel.InstanceModel.Name}:{space.Name}", syncJobCompositeModel);
            var octopusList = await octopusRepository.GetAllEnvironmentsForSpaceAsync(syncJobCompositeModel.InstanceModel, space);
            await LogInformation($"{octopusList.Count} environments(s) found in {syncJobCompositeModel.InstanceModel.Name}:{space.Name}", syncJobCompositeModel);

            var returnDictionary = new Dictionary<string, EnvironmentModel>();
            foreach (var item in octopusList)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }

                await LogInformation($"Checking to see if environment {item.OctopusId}:{item.Name} already exists", syncJobCompositeModel);
                var itemModel = await storeHubDataAdapter.GetByOctopusIdAsync<EnvironmentModel>(item.OctopusId);
                await LogInformation($"{(itemModel != null ? "Environment already exists, updating" : "Unable to find environment, creating")}", syncJobCompositeModel);
                item.Id = itemModel?.Id ?? 0;

                await LogInformation($"Saving environment {item.OctopusId}:{item.Name} to the database", syncJobCompositeModel);
                var modelToTrack = item.Id > 0 ? await storeHubDataAdapter.UpdateAsync(item) : await storeHubDataAdapter.InsertAsync(item);

                await LogInformation($"Adding environment {item.OctopusId}:{item.Name} to our sync dictionary for faster lookup", syncJobCompositeModel);
                returnDictionary.Add(item.OctopusId, modelToTrack);
            }

            return returnDictionary;
        }        

        private async Task LogInformation(string message, SyncJobCompositeModel syncJobCompositeModel)
        {
            var formattedMessage = $"{syncJobCompositeModel.GetMessagePrefix()}{message}";
            logger.LogInformation(formattedMessage);
            await storeHubDataAdapter.InsertAsync(syncLogModelFactory.MakeInformationLog(formattedMessage, syncJobCompositeModel.SyncModel.Id));
        }
    }
}
