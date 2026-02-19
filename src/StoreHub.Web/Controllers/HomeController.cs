using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using StoreHub.Web.BusinessLogic.Factories;
using StoreHub.Web.Core.Configuration;
using StoreHub.Web.Core.Constants;
using StoreHub.Web.Core.Models;
using StoreHub.Web.Core.Models.ViewModels;
using StoreHub.Web.DataAccess;

namespace StoreHub.Web.Controllers
{
    public class HomeController (ILogger<HomeController> logger,
            IStoreHubDataAdapter storeHubDataAdapter,
            ISyncModelFactory syncModelFactory,            
            IMetricConfiguration metricConfiguration)
        : Controller
    {        
        public async Task<IActionResult> Index(int currentPage = 1, int rowsPerPage = 10, string sortColumn = "Name", bool isAsc = true)
        {            
            var allInstances = await storeHubDataAdapter.GetAllAsync<InstanceModel>(currentPage, rowsPerPage, sortColumn, isAsc);      

            if (allInstances.Items.Count == 0 && string.IsNullOrWhiteSpace(metricConfiguration.DefaultInstanceUrl) == false && metricConfiguration.DefaultInstanceUrl != "blah")  
            {
                logger.LogDebug("No instances found, creating default instance");
                var defaultInstance = new InstanceModel
                {
                    Name = "Default Instance",
                    OctopusId = metricConfiguration.DefaultInstanceId,
                    Url = metricConfiguration.DefaultInstanceUrl,
                    ApiKey = metricConfiguration.DefaultInstanceApiKey
                };

                await storeHubDataAdapter.InsertAsync(defaultInstance);

                allInstances = await storeHubDataAdapter.GetAllAsync<InstanceModel>(currentPage, rowsPerPage, sortColumn, isAsc);
            }
            else
            {
                logger.LogDebug("Instances found");
            }

            return View(allInstances);
        }

        public async Task<IActionResult> StartSync(int id)
        {
            var instance = await storeHubDataAdapter.GetByIdAsync<InstanceModel>(id);

            var syncWhereClause = $"Where InstanceId = {id} and state = '{SyncState.Completed}'";
            var previousSync = await storeHubDataAdapter.GetFirstRecordAsync<SyncModel>(syncWhereClause, sortColumn: "Completed", isAsc: false);

            var newSync = syncModelFactory.CreateModel(id, instance.Name, previousSync);

            await storeHubDataAdapter.InsertAsync(newSync);

            return RedirectToAction("Index", "Sync");
        }

        public IActionResult AddInstance()
        {
            var instance = new InstanceModel();

            return View("InstanceMaintenance", instance);
        }

        public async Task<IActionResult> EditInstance(int id)
        {
            var instance = await storeHubDataAdapter.GetByIdAsync<InstanceModel>(id);

            return View("InstanceMaintenance", instance);
        }

        [HttpPost]
        public async Task<IActionResult> Save(InstanceModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View("InstanceMaintenance", model);
            }

            if (model.Id > 0)
            {
                await storeHubDataAdapter.UpdateAsync(model);
            }
            else
            {
                await storeHubDataAdapter.InsertAsync(model);
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
