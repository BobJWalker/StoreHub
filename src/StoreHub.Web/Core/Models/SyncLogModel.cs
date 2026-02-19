using System;
using Dapper;

namespace StoreHub.Web.Core.Models
{
    [Table("SyncLog")]
    public class SyncLogModel : BaseModel
    {
        public int SyncId { get; set; }
        public DateTime Created { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
    }
}
