using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using FunctionApp.Entities;

namespace FunctionApp
{
    public class FetchDataFunction
    {
        private readonly ILogger _logger;

        public FetchDataFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LogItemsFunction>();
        }

        [Function("FetchDataFunction")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] FunctionContext context)
        {
            _logger.LogInformation($"FetchData function trigger function executed at: {DateTime.UtcNow}");

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(Environment.GetEnvironmentVariable("Url"));

            var logEntity = new LogEntity()
            {
                Status = response.IsSuccessStatusCode ? "Success" : "Failure"
            };

            var tableClient = new TableClient(Environment.GetEnvironmentVariable("StorageConnectionString"), Environment.GetEnvironmentVariable("TableName"));
            await tableClient.CreateIfNotExistsAsync();
            await tableClient.AddEntityAsync<LogEntity>(logEntity);

            // Save payload in Azure Storage blob container if attempt was successful
            if (response.IsSuccessStatusCode)
            {
                var blobContainerClient = new BlobContainerClient(Environment.GetEnvironmentVariable("StorageConnectionString"), Environment.GetEnvironmentVariable("BlobContainerName"));
                await blobContainerClient.CreateIfNotExistsAsync();
                var blobName = $"{logEntity.RowKey}.json";
                var responseBody = await response.Content.ReadAsStreamAsync();

                await blobContainerClient.UploadBlobAsync(blobName, responseBody);
            }
        }
    }
}