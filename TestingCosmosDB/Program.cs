using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

namespace CosmosDBQueryExample
{
    public class Program
    {
        private static readonly string EndpointUri = "https://cicobolocosmosdb.documents.azure.com:443/";
        private static readonly string PrimaryKey = "YayHFI4gpBcQxdJDnAgDb1Pf5yZeyY2LSqaaleNhD8HW14I4bzfAoGKoOlru1dyK5MIr4l8o8I1VACDbjU3Pvg==";
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;

        public static void Main(string[] args)
        {
            Program program = new Program();
            Task.Run(async () => { await program.StartAsync(); }).GetAwaiter().GetResult();
        }

        private async Task StartAsync()
        {
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            this.database = this.cosmosClient.GetDatabase("IoTCosmosDB");
            this.container = this.database.GetContainer("Devices");

            await QueryAndPrintDataAsync();
        }

        private async Task QueryAndPrintDataAsync()
        {
            var sqlQueryText = "SELECT * FROM c";
            Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<MyDocument> queryResultSetIterator = this.container.GetItemQueryIterator<MyDocument>(queryDefinition);

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<MyDocument> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (MyDocument item in currentResultSet)
                {
                    Console.WriteLine($"Id: {item.Id}, Staus: {item.Status}, Payload: {item.Payload}");
                }
            }
        }

        public class MyDocument
        {
            public string Id { get; set; }
            public string? Status { get; set; }
            public PayloadWrapper Payload { get; set; }
        }

        public class PayloadWrapper
        {
            public string Message { get; set; }
            public object Payload { get; set; } // Use 'object' if the payload can be of any type
        }
    }
}
