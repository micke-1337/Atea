using System.Net;
using Azure.Data.Tables;
using FunctionApp.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class LogItemsFunction
    {
        private readonly ILogger _logger;

        public LogItemsFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LogItemsFunction>();
        }

        [Function("LogItems")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("LogItems function processed a request.");

            if (!DateTime.TryParse(req.Query["from"], out DateTime startTime))
            {
                var error = req.CreateResponse(HttpStatusCode.BadRequest);
                error.WriteString("Invalid 'from' time parameter.");
                return error;
            }

            if (!DateTime.TryParse(req.Query["to"], out DateTime endTime))
            {
                var error = req.CreateResponse(HttpStatusCode.BadRequest);
                error.WriteString("Invalid 'to' time parameter.");
                return error;
            }

            var tableClient = new TableClient(Environment.GetEnvironmentVariable("StorageConnectionString"), Environment.GetEnvironmentVariable("TableName"));
            var logItems = tableClient.Query<LogEntity>(x => x.Timestamp >= startTime && x.Timestamp <= endTime).OrderBy(x => x.Timestamp);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync<IEnumerable<LogEntity>>(logItems);

            return response;
        }
    }
}
