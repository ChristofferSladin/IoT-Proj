using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CosmosdbDeviceTrigger
{
    public class CosmosDemo
    {
        private readonly ILogger _logger;

        public CosmosDemo(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CosmosDemo>();
        }

        [Function("TestFunction")]
        public void Run([CosmosDBTrigger(
            databaseName: "IoTCosmosDB",
            containerName: "Devices",
            Connection = "connectionstringcosmosbd",
            LeaseContainerName = "leases")] IReadOnlyList<MyDocument> input)
        {
            if (input != null && input.Count > 0)
            {
                _logger.LogInformation("Documents modified: " + input.Count);
                _logger.LogInformation("First document Id: " + input[0].Id);
            }
        }
    }

    public class MyDocument
    {
        public string Id { get; set; }
        public int Status { get; set; }
        public PayloadWrapper Payload { get; set; }
    }

    public class PayloadWrapper
    {
        public string Message { get; set; }
        public object Payload { get; set; } // Use 'object' if the payload can be of any type
    }
}
