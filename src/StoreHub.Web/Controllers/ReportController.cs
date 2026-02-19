using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreHub.Web.Core.Models;
using StoreHub.Web.Core.Models.ViewModels;
using StoreHub.Web.DataAccess;

namespace StoreHub.Web.Controllers
{
    public class ReportController(IStoreHubDataAdapter storeHubDataAdapter)
        : Controller
    {        
        public async Task<IActionResult> Index(int instanceId)
        {
            var instanceModel = await storeHubDataAdapter.GetByIdAsync<InstanceModel>(instanceId);
            var spaceList = await storeHubDataAdapter.GetAllByParentIdAsync<SpaceModel>(currentPageNumber: 1, rowsPerPage: int.MaxValue, "Name", true, "InstanceId", instanceId);

            var viewModel = new ReportingViewModel
            {
                Instance = instanceModel,
                SpaceList = spaceList.Items
            };

            return View(viewModel);
        }
    }
}