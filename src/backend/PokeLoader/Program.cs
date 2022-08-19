using NRediSearch;
using NReJSON;
using StackExchange.Redis;
using System;
using System.IO;
using System.Linq;
using static NRediSearch.Client;

public class Program
{
    public static void Main(string[] args)
    {
        string REDIS_CONNECTION = Environment.GetEnvironmentVariable("REDIS_CONNECTION");
        string CACHE_DIRECTORY = Environment.GetEnvironmentVariable("CACHE_DIRECTORY");

        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(REDIS_CONNECTION);
        IDatabase db = redis.GetDatabase();

        foreach (string resourceDir in Directory.GetDirectories(CACHE_DIRECTORY))
        {
            Uri resourceUri  = new Uri(resourceDir);
            string index = resourceUri.Segments.Last();
            DropCreateIndex(db, index);

            foreach (string itemDir in Directory.GetDirectories(resourceDir))
            {
                Uri itemUri = new Uri(itemDir);
                string id = itemUri.Segments.Last();
                string[] files = Directory.GetFiles(itemDir);
                string json = File.ReadAllText(files.SingleOrDefault(x => x.Contains("index.json")));
                RedisKey key = new RedisKey($"{index}:{id}");
                OperationResult result = db.JsonSet(key, json);

                if (!result.IsSuccess)
                {
                    Console.WriteLine($"{key} failed to write.");
                }
            }
        }

        Console.WriteLine("Done!");
    }

    public static void DropCreateIndex(IDatabase db, string index)
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