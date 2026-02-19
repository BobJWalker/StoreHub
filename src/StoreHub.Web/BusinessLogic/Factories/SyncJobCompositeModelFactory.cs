using System.Threading.Tasks;
using StoreHub.Web.Core.Models;
using StoreHub.Web.Core.Models.CompositeModels;
using StoreHub.Web.DataAccess;

namespace StoreHub.Web.BusinessLogic.Factories
{
    public interface ISyncJobCompositeModelFactory
    {
        Task<SyncJobCompositeModel> MakeSyncJobCompositeModelAsync(SyncModel syncModel);
    }

    public class SyncJobCompositeModelFactory(IStoreHubDataAdapter instanceRepository) : ISyncJobCompositeModelFactory
    {
        public async Task<SyncJobCompositeModel> MakeSyncJobCompositeModelAsync(SyncModel syncModel)
        {
            var instance = await instanceRepository.GetByIdAsync<InstanceModel>(syncModel.InstanceId);

            return new SyncJobCompositeModel
            {
                SyncModel = syncModel,
                InstanceModel = instance
            };
        }
    }
}
