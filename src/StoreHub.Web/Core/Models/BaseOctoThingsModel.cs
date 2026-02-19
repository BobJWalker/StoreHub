using System.ComponentModel.DataAnnotations.Schema;

namespace StoreHub.Web.Core.Models
{
    public class BaseOctoThingsModel : BaseOctopusModel
    {
        public int SpaceId { get; set; }
        public string Name { get; set; }
    }
}