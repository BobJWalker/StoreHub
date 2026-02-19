using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreHub.Web.Core.Models;
using StoreHub.Web.DataAccess;

namespace StoreHub.Web.Controllers
{
    public class SyncController(IStoreHubDataAdapter storeHubDataAdapter) : Controller
    {
        public async Task<IActionResult> Index(int currentPage = 1, int rowsPerPage = 10, string sortColumn = "Name", bool isAsc = true)
        {
            var pagedSyncView = await storeHubDataAdapter.GetAllAsync<SyncModel>(currentPage, rowsPerPage, sortColumn, isAsc);

            return View(pagedSyncView);
        }
    }
}
