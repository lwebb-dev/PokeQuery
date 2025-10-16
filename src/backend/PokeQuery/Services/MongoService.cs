using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;

namespace PokeQuery.Services;

public class MongoService : IQueryService
{
    private readonly IMongoDatabase database;

    public MongoService(IConfiguration configuration)
    {
        string mongoConnection = configuration["MONGO_CONNECTION"];
        MongoClient client = new MongoClient(mongoConnection);
        this.database = client.GetDatabase("poke_mongo");
    }

    public async Task<IEnumerable<string>> QueryIndexJsonAsync(string index, string query)
    {
        IAsyncCursor<string> collections = await database.ListCollectionNamesAsync();
        List<string> matchedCollections = collections.ToList()
            .Where(name => name.StartsWith(index)).ToList();
        List<string> results = new List<string>();

        foreach (string collectionName in matchedCollections)
        {
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(collectionName);
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter
                .Regex("name", new BsonRegularExpression($".*{query}.*", "i"));
            List<BsonDocument> docs = await collection.Find(filter).ToListAsync();
            results.AddRange(docs.Select(doc => doc.ToJson()));
        }

        return results;
    }

    public async Task<IEnumerable<string>> GetJsonResultsByPatternAsync(string pattern)
    {
        // Strip the Redis-style ":*" pattern for MongoDB collection lookup
        string collectionPrefix = pattern.Replace(":*", "");

        IAsyncCursor<string> collections = await database.ListCollectionNamesAsync();
        List<string> matchedCollections = collections.ToList()
            .Where(name => name == collectionPrefix || name.StartsWith(collectionPrefix + "_")).ToList();
        List<string> results = new List<string>();

        foreach (string collectionName in matchedCollections)
        {
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(collectionName);
            List<BsonDocument> docs = await collection.Find(FilterDefinition<BsonDocument>.Empty).ToListAsync();
            results.AddRange(docs.Select(doc => doc.ToJson()));
        }

        return results;
    }

    public async Task<string> GetJsonResultAsync(string prefix, string[] paths = null)
    {
        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(prefix);

        BsonDocument doc = await collection.Find(FilterDefinition<BsonDocument>.Empty).FirstOrDefaultAsync();

        if (doc == null)
        {
            return null;
        }

        if (paths != null && paths.Length > 0)
        {
            BsonDocument filtered = new BsonDocument();

            foreach (string path in paths)
            {
                if (doc.Contains(path))
                {
                    filtered[path] = doc[path];
                }
            }

            return filtered.ToJson();
        }

        return doc.ToJson();
    }
}
