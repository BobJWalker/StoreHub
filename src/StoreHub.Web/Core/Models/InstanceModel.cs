using System.ComponentModel.DataAnnotations.Schema;

namespace StoreHub.Web.Core.Models
{
    [Table("Instance")]
    public class InstanceModel : BaseOctopusModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string ApiKey { get; set; }
    }
}