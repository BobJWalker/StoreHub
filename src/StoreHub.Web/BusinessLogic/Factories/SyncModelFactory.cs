using System;
using StoreHub.Web.Core.Constants;
using StoreHub.Web.Core.Models;

namespace StoreHub.Web.BusinessLogic.Factories
{
    public interface ISyncModelFactory
    {
        SyncModel CreateModel(int instanceId, string instanceName, SyncModel previousSync);
    }

    public class SyncModelFactory : ISyncModelFactory
    {
        public SyncModel CreateModel(int instanceId, string instanceName, SyncModel previousSync)
        {
            return new SyncModel
            {
                InstanceId = instanceId,
                Created = DateTime.Now,
                Name = $"Sync for {instanceName}",
                State = SyncState.Queued,
                SearchStartDate = previousSync?.Started
            };
        }
    }
}
