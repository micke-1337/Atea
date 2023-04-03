using Azure.Data.Tables;
using Azure;

namespace FunctionApp.Entities
{
    public record LogEntity : ITableEntity
    {
        public LogEntity()
        {
            PartitionKey = "default";
            RowKey = Guid.NewGuid().ToString();
        }

        public string RowKey { get; set; } = default!;

        public string PartitionKey { get; set; } = default!;

        public string Status { get; set; } = default!;

        public ETag ETag { get; set; } = default!;

        public DateTimeOffset? Timestamp { get; set; } = default!;
    }
}
