using Microsoft.Extensions.Logging;
using Moq;
using StoreHub.Web.BusinessLogic.Factories;
using StoreHub.Web.DataAccess;
using StoreHub.Web.BusinessLogic.Syncers;

namespace StoreHub.Web.Tests.BusinessLogic.Syncers
{
    [TestFixture]
    public class EventSyncerTest
    {
        private Mock<ILogger<EventSyncer>> _loggerMock;        
        private Mock<IOctopusRepository> _octopusRepositoryMock;
        private Mock<IStoreHubDataAdapter> _genericRepoMock;        
        private Mock<ISyncLogModelFactory> _syncLogModelFactoryMock;
        private EventSyncer _eventSyncer;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<EventSyncer>>();            
            _octopusRepositoryMock = new Mock<IOctopusRepository>();
            _genericRepoMock = new Mock<IStoreHubDataAdapter>();            
            _syncLogModelFactoryMock = new Mock<ISyncLogModelFactory>();

            _eventSyncer = new EventSyncer(
                _loggerMock.Object,                
                _octopusRepositoryMock.Object,
                _genericRepoMock.Object,                
                _syncLogModelFactoryMock.Object
            );
        }
    }
}