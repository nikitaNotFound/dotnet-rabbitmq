using Microsoft.Extensions.Options;

using MongoDB.Driver;

using publisher.Domain.Models;
using publisher.Domain.Options;

namespace publisher.Domain
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbOptions> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoCollection<LoanRequest> LoanRequests
            => _database.GetCollection<LoanRequest>(MongoDbCollectionNames.LoanRequests);
    }

    public static class MongoDbCollectionNames
    {
        public const string LoanRequests = "LoanRequests";
    }
}