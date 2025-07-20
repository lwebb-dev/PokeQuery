using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace PokeLoader.Loaders;

public sealed class MongoLoader : ILoader
{
    private readonly IMongoDatabase database;

    public MongoLoader(string mongoConnectionString)
    {
        var client = new MongoClient(mongoConnectionString);
        database = client.GetDatabase("poke_mongo");
    }

    public void Load(Dictionary<string, Dictionary<int, string>> indexDictionary)
    {
        Console.WriteLine("Loading JSON Data Using MongoLoader...");

        foreach (var indexKvp in indexDictionary)
        {
            string collectionName = indexKvp.Key;
            var collection = database.GetCollection<BsonDocument>(collectionName);
            DropAndCreateCollection(collectionName);

            var documents = new List<BsonDocument>();
            foreach (var keyedJsonKvp in indexKvp.Value)
            {
                int id = keyedJsonKvp.Key;
                string json = keyedJsonKvp.Value;
                var doc = BsonDocument.Parse(json);
                doc["id"] = id;
                documents.Add(doc);
            }

            if (documents.Count > 0)
            {
                collection.InsertMany(documents);
            }
        }
    }

    private void DropAndCreateCollection(string collectionName)
    {
        database.DropCollection(collectionName);
        database.CreateCollection(collectionName);
    }
}
