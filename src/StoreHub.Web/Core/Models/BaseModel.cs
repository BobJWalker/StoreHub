using System.ComponentModel.DataAnnotations;

namespace StoreHub.Web.Core.Models
{
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }
    }
}
