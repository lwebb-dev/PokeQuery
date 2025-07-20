using NRediSearch;
using NReJSON;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using static NRediSearch.Client;

namespace PokeLoader.Loaders;

public sealed class RedisLoader : ILoader
{
    private readonly ConnectionMultiplexer redis;
    private readonly IDatabase db;

    public RedisLoader(string redisConnectionString)
    {
        this.redis = ConnectionMultiplexer.Connect(redisConnectionString);
        this.db = redis.GetDatabase();
    }

    public void Load(Dictionary<string, Dictionary<int, string>> indexDictionary)
    {
        Console.WriteLine("Loading JSON Data Using RedisLoader...");

        foreach (KeyValuePair<string, Dictionary<int, string>> indexKvp in indexDictionary)
        {
            string index = indexKvp.Key;
            DropCreateIndex(db, index);

            foreach (KeyValuePair<int, string> keyedJsonKvp in indexKvp.Value)
            {
                int id = keyedJsonKvp.Key;
                string json = keyedJsonKvp.Value;
                RedisKey key = new RedisKey($"{index}:{id}");
                OperationResult result = db.JsonSet(key, json);

                if (!result.IsSuccess)
                {
                    Console.WriteLine($"{key} failed to write.");
                }
            }
        }
    }




    //    foreach (string resourceDir in Directory.GetDirectories(CACHE_DIRECTORY))
    //    {
    //        Uri resourceUri = new Uri(resourceDir);
    //        string index = resourceUri.Segments.Last();

    //        if (!IndexHelper.ValidIndexes.Contains(index))
    //            continue;

    //        DropCreateIndex(db, index);

    //        foreach (string itemDir in Directory.GetDirectories(resourceDir))
    //        {
    //            Uri itemUri = new Uri(itemDir);
    //            string id = itemUri.Segments.Last();
    //            string[] files = Directory.GetFiles(itemDir);
    //            string rawJson = File.ReadAllText(files.SingleOrDefault(x => x.Contains("index.json")));

    //            if (IndexHelper.GetPresentationJsonFromIndex(index, rawJson, out string json))
    //            {
    //                RedisKey key = new RedisKey($"{index}:{id}");
    //                OperationResult result = db.JsonSet(key, json);

    //                if (!result.IsSuccess)
    //                {
    //                    Console.WriteLine($"{key} failed to write.");
    //                }
    //            }
    //        }
    //    }

    private void DropCreateIndex(IDatabase db, string index)
    {
        string namedIndex = $"idx:{index}";

        try
        {
            db.Execute("FT.DROPINDEX", namedIndex);
        }
        catch
        {
        }

        var schema = new Schema()
            .AddTextField("$.name")
            .AddSortableNumericField("id");

        var options = new ConfiguredIndexOptions(
            new IndexDefinition(
                type: IndexDefinition.IndexType.Json,
                prefixes: new[] { $"{index}:" }
            )
        );

        Client searchNameClient = new Client(namedIndex, db);
        bool result = searchNameClient.CreateIndex(schema, options);
        Console.WriteLine($"{namedIndex}: {result}");
    }
}
