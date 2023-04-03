using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace FunctionApp
{
    public class PayloadFunction
    {
        private readonly ILogger _logger;

        public PayloadFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PayloadFunction>();
        }

        [Function("Payload")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Logitems/{logEntry:guid}" )] HttpRequestData req, Guid logEntry)
        {
            _logger.LogInformation("Payload function processed a request.");

            var blobContainerClient = new BlobContainerClient(Environment.GetEnvironmentVariable("StorageConnectionString"), Environment.GetEnvironmentVariable("BlobContainerName"));
            var blobName = $"{logEntry}.json";
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            if (await blobClient.ExistsAsync() == false)
                return req.CreateResponse(HttpStatusCode.NotFound);
                
            BlobDownloadResult blob = await blobClient.DownloadContentAsync();
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteBytesAsync((blob.Content.ToArray()));

            return response;
        }
    }
}
