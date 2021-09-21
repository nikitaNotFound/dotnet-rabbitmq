using consumer.Domain.Models;
using consumer.Domain.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace consumer.Domain
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbOptions> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoCollection<LoanProcessingInfo> LoanProcessingInfos
            => _database.GetCollection<LoanProcessingInfo>(MongoDbCollectionNames.LoanRequests);
    }

    public static class MongoDbCollectionNames
    {
        public const string LoanRequests = "LoanProcessingInfos";
    }
}