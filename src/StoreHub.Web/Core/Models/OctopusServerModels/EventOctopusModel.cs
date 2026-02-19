using System.Collections.Generic;

namespace StoreHub.Web.Core.Models.OctopusServerModels
{
    public class EventOctopusModel : BaseOctopusServerModel
    {
        public string SpaceId { get; set; }
        public string Message { get; set; }
        public List<string> RelatedDocumentIds { get; set; }
    }
}
