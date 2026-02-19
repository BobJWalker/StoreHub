using Microsoft.Extensions.Configuration;
using System;

namespace StoreHub.Web.Core.Configuration
{
    public interface IMetricConfiguration
    {
        string ConnectionString { get; set; }
        string DefaultInstanceUrl { get; set; }
        string DefaultInstanceApiKey { get; set; }
        string DefaultInstanceId { get; set; }
        string EnvironmentName { get; set; }
    }

    public class MetricConfiguration : IMetricConfiguration
    {
        public string ConnectionString { get; set; }
        public string DefaultInstanceUrl {get; set;}
        public string DefaultInstanceApiKey {get; set;}
        public string DefaultInstanceId {get; set;}
        public string EnvironmentName { get; set; }

        public MetricConfiguration()
        {
            ConnectionString = Environment.GetEnvironmentVariable("STOREHUB_CONNECTION_STRING");
            DefaultInstanceUrl = Environment.GetEnvironmentVariable("STOREHUB_INSTANCE_URL");
            DefaultInstanceApiKey = Environment.GetEnvironmentVariable("STOREHUB_INSTANCE_API_KEY");
            DefaultInstanceId = Environment.GetEnvironmentVariable("STOREHUB_INSTANCE_ID");
            EnvironmentName = Environment.GetEnvironmentVariable("STOREHUB_ENVIRONMENT");
        }
    }
}
