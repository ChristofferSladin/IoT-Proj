using System;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Azure.Messaging.EventHubs;
using System.Collections.Concurrent;
using Microsoft.Azure.Cosmos.Linq;

public class Function1
{
    private readonly CosmosClient _cosmosClient;

    public Function1(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
    }

    [Function("EventHubTriggerFunction")]
    public async Task Run([EventHubTrigger("iothub-ehub-cs-iot-hea-25230151-265c546124", Connection = "EventHubConnectionAppSetting")] EventData[] events, FunctionContext context)
    {
        var logger = context.GetLogger("EventHubTriggerFunction");
        var container = _cosmosClient.GetContainer("IoTCosmosDB", "Devices");

        foreach (EventData eventData in events)
        {
            var messageBody = Encoding.UTF8.GetString(eventData.Body.ToArray());
            logger.LogInformation($"Received message: {messageBody}");

            // Determine if the message is a "Start" or "Stop" command
            if (messageBody == "started" || messageBody == "stopped")
            {
                MyDirectMethodDocument directMethodData = new MyDirectMethodDocument
                {
                    Id = Guid.NewGuid().ToString(),
                    Status = messageBody
                };

                await container.CreateItemAsync(directMethodData, new PartitionKey(directMethodData.Id));
                logger.LogInformation($"Direct Method '{messageBody}' saved to Cosmos DB with Id: {directMethodData.Id}");
            }
            else
            {
                MyDocument data = JsonConvert.DeserializeObject<MyDocument>(messageBody);

                await container.CreateItemAsync(data, new PartitionKey(data.Id));
                logger.LogInformation($"Saved to Cosmos DB: {data.Id}");
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

    public class MyDirectMethodDocument
    {
        public string Id { get; set; }
        public string Status { get; set; }
    }
}
