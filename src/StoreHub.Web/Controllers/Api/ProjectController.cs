using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreHub.Web.Core.Models;
using StoreHub.Web.Core.Models.ViewModels;
using StoreHub.Web.DataAccess;

namespace StoreHub.Web.Controllers.Api
{
    [ApiController]
    [Route("api/instances/{instanceId}/spaces/{spaceId}/projects")]
    public class ProjectController(IStoreHubDataAdapter repository) : ControllerBase
    {
        [HttpGet]        
        public Task<PagedViewModel<ProjectModel>> GetAll(int spaceId, int currentPage = 1, int rowsPerPage = 10, string sortColumn = "Name", bool isAsc = true)
        {
            return repository.GetAllByParentIdAsync<ProjectModel>(currentPage, rowsPerPage, sortColumn, isAsc, "SpaceId", spaceId);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<ProjectModel> GetById(int id)
        {
            return repository.GetByIdAsync<ProjectModel>(id);
        }
        
        [HttpPost]        
        public Task<ProjectModel> Insert(int spaceId, ProjectModel model)
        {
            model.SpaceId = spaceId;

            return repository.InsertAsync(model);
        }
        
        [HttpPut]
        [Route("{id}")]
        public Task<ProjectModel> Update(int spaceId, int id, ProjectModel model)
        {
            model.SpaceId = spaceId;
            model.Id = id;
            return repository.UpdateAsync(model);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public Task Delete(int id)
        {
            return repository.DeleteAsync<ProjectModel>(id);
        }
    }
}