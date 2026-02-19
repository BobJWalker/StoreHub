using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreHub.Web.Core.Models;
using StoreHub.Web.Core.Models.ViewModels;
using StoreHub.Web.DataAccess;

namespace StoreHub.Web.Controllers.Api
{
    [ApiController]
    [Route("api/instances")]
    public class InstanceController(IStoreHubDataAdapter repository) : ControllerBase
    {
        [HttpGet]        
        public Task<PagedViewModel<InstanceModel>> GetAll(int currentPage = 1, int rowsPerPage = 10, string sortColumn = "Name", bool isAsc = true)
        {
            return repository.GetAllAsync<InstanceModel>(currentPage, rowsPerPage, sortColumn, isAsc);
        }
        
        [HttpGet]
        [Route("{id}")]
        public Task<InstanceModel> GetById(int id)
        {
            return repository.GetByIdAsync<InstanceModel>(id);
        }
        
        [HttpPost]
        public Task<InstanceModel> Insert(InstanceModel model)
        {
            return repository.InsertAsync(model);
        }
        
        [HttpPut]
        [Route("{id}")]
        public Task<InstanceModel> Update(InstanceModel model)
        {
            return repository.UpdateAsync(model);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public Task Delete(int id)
        {
            return repository.DeleteAsync<InstanceModel>(id);
        }
    }
}