using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreHub.Web.Core.Models;
using StoreHub.Web.Core.Models.ViewModels;
using StoreHub.Web.DataAccess;

namespace StoreHub.Web.Controllers.Api
{
    [ApiController]
    [Route("api/instances/{instanceId}/spaces/{spaceId}/environments")]
    public class EnvironmentController(IStoreHubDataAdapter repository) : ControllerBase
    {
        [HttpGet]        
        public Task<PagedViewModel<EnvironmentModel>> GetAll(int spaceId, int currentPage = 1, int rowsPerPage = 10, string sortColumn = "Start", bool isAsc = true)
        {
            return repository.GetAllByParentIdAsync<EnvironmentModel>(currentPage, rowsPerPage, sortColumn, isAsc, "SpaceId", spaceId);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<EnvironmentModel> GetById(int id)
        {
            return repository.GetByIdAsync<EnvironmentModel>(id);
        }
        
        [HttpPost]        
        public Task<EnvironmentModel> Insert(int spaceId, EnvironmentModel model)
        {
            model.SpaceId = spaceId;

            return repository.InsertAsync<EnvironmentModel>(model);
        }
        
        [HttpPut]
        [Route("{id}")]
        public Task<EnvironmentModel> Update(int spaceId, int id, EnvironmentModel model)
        {
            model.SpaceId = spaceId;
            model.Id = id;
            return repository.UpdateAsync<EnvironmentModel>(model);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public Task Delete(int id)
        {
            return repository.DeleteAsync<EnvironmentModel>(id);
        }
    }
}