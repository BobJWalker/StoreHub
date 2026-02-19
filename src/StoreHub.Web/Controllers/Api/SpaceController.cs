using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreHub.Web.Core.Models;
using StoreHub.Web.Core.Models.ViewModels;
using StoreHub.Web.DataAccess;

namespace StoreHub.Web.Controllers.Api
{
    [ApiController]
    [Route("api/instances/{instanceId}/spaces")]
    public class SpaceController(IStoreHubDataAdapter repository) : ControllerBase
    {
        [HttpGet]
        public Task<PagedViewModel<SpaceModel>> GetAll(int instanceId, int currentPage = 1, int rowsPerPage = 10, string sortColumn = "Start", bool isAsc = true)
        {
            return repository.GetAllByParentIdAsync<SpaceModel>(currentPage, rowsPerPage, sortColumn, isAsc, "InstanceId", instanceId);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<SpaceModel> GetById(int id)
        {
            return repository.GetByIdAsync<SpaceModel>(id);
        }
        
        [HttpPost]        
        public Task<SpaceModel> Insert(int instanceId, SpaceModel model)
        {
            model.InstanceId = instanceId;

            return repository.InsertAsync(model);
        }
        
        [HttpPut]
        [Route("{id}")]
        public Task<SpaceModel> Update(int instanceId, int id, SpaceModel model)
        {
            model.InstanceId = instanceId;
            model.Id = id;
            return repository.UpdateAsync(model);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public Task Delete(int id)
        {
            return repository.DeleteAsync<SpaceModel>(id);
        }
    }
}