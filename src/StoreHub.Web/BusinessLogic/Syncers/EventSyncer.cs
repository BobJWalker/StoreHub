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
    public interface IEventSyncer
    {
         Task ProcessDeploymentsSinceLastSync(SyncJobCompositeModel syncJobCompositeModel, CancellationToken stoppingToken);
    }

    public class EventSyncer (
        ILogger<EventSyncer> logger,        
        IOctopusRepository octopusRepository,
        IStoreHubDataAdapter storeHubDataAdapter,
        ISyncLogModelFactory syncLogModelFactory) : IEventSyncer
    {
        public async Task ProcessDeploymentsSinceLastSync(SyncJobCompositeModel syncJobCompositeModel, CancellationToken stoppingToken)
        {
            var startIndex = 0;
            var canContinue = true;

            await LogInformation($"Finding all deployments since {syncJobCompositeModel.SyncModel.SearchStartDate}", syncJobCompositeModel);            

            while (canContinue)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }

                await LogInformation($"Getting the next results at {startIndex}", syncJobCompositeModel);
                var eventResults = await octopusRepository.GetAllEvents(syncJobCompositeModel.InstanceModel, syncJobCompositeModel.SyncModel, startIndex);

                foreach (var octopusEvent in eventResults.Items)
                {
                    if (stoppingToken.IsCancellationRequested)
                    {
                        break;
                    }

                    var spaceId = octopusEvent.SpaceId;
                    var space = syncJobCompositeModel.SpaceDictionary[spaceId];

                    var projectId = octopusEvent.RelatedDocumentIds.First(x => x.StartsWith("Projects"));
                    var project = syncJobCompositeModel.ProjectDictionary[projectId];

                    var releaseId = octopusEvent.RelatedDocumentIds.First(x => x.StartsWith("Release"));                    
                    var releaseModelToTrack = await storeHubDataAdapter.GetByIdAsync<ReleaseModel>(int.Parse(releaseId));

                        var deploymentId = octopusEvent.RelatedDocumentIds.First(x => x.StartsWith("DeploymentId"));
                        var deploymentModel = await octopusRepository.GetSpecificDeployment(syncJobCompositeModel.InstanceModel, space, releaseModelToTrack, deploymentId, syncJobCompositeModel.EnvironmentDictionary, syncJobCompositeModel.TenantDictionary);

                        if (deploymentModel != null)
                        {
                            var itemModel = await storeHubDataAdapter.GetByIdAsync<DeploymentModel>(deploymentModel.Id);
                            await LogInformation($"{(itemModel != null ? "Deployment already exists, updating" : "Unable to find deployment, creating")}", syncJobCompositeModel);
                            deploymentModel.Id = itemModel?.Id ?? 0;

                            await LogInformation($"Saving deployment {deploymentModel.OctopusId} to the database", syncJobCompositeModel);
                            var modelToTrack = deploymentModel.Id > 0 ? await storeHubDataAdapter.UpdateAsync(deploymentModel) : await storeHubDataAdapter.InsertAsync(deploymentModel);
                        }
                }

                canContinue = eventResults.Items.Count > 0;
                startIndex += 10;
            }
            
        }

        private async Task LogInformation(string message, SyncJobCompositeModel syncJobCompositeModel)
        {
            var formattedMessage = $"{syncJobCompositeModel.GetMessagePrefix()}{message}";
            logger.LogInformation(formattedMessage);
            await storeHubDataAdapter.InsertAsync(syncLogModelFactory.MakeInformationLog(formattedMessage, syncJobCompositeModel.SyncModel.Id));
        }
    }
}
