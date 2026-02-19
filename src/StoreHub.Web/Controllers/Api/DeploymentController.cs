using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreHub.Web.Core.Models;
using StoreHub.Web.Core.Models.ViewModels;
using StoreHub.Web.DataAccess;

namespace StoreHub.Web.Controllers.Api
{
    [ApiController]
    [Route("api/instances/{instanceId}/spaces/{spaceId}/projects/{projectId}/releases/{releaseId}/deployments")]
    public class DeploymentController(IStoreHubDataAdapter repository) : ControllerBase
    {        
        [HttpGet]        
        public Task<PagedViewModel<DeploymentModel>> GetAll(int releaseId, int currentPage = 1, int rowsPerPage = 10, string sortColumn = "Start", bool isAsc = true)
        {
            return repository.GetAllByParentIdAsync<DeploymentModel>(currentPage, rowsPerPage, sortColumn, isAsc, "releaseId", releaseId);
        }
        
        [HttpGet]
        [Route("{id}")]
        public Task<DeploymentModel> GetById(int id)
        {
            return repository.GetByIdAsync<DeploymentModel>(id);
        }
        
        [HttpPost]        
        public Task<DeploymentModel> Insert(int releaseId, DeploymentModel model)
        {
            model.ReleaseId = releaseId;

            return repository.InsertAsync(model);
        }
        
        [HttpPut]
        [Route("{id}")]
        public Task<DeploymentModel> Update(int releaseId, int id, DeploymentModel model)
        {
            model.ReleaseId = releaseId;
            model.Id = id;
            return repository.UpdateAsync(model);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public Task Delete(int id)
        {
            return repository.DeleteAsync<DeploymentModel>(id);
        }
    }
}