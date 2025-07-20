using PokeLoader.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeLoader
{
    public sealed class Program
    {
        private static string REDIS_CONNECTION;
        private static string MONGO_CONNECTION;
        private static string CACHE_DIRECTORY;

        private static Dictionary<string, Dictionary<int, string>> IndexDictionary;

        public static void Main(string[] args)
        {
            REDIS_CONNECTION = args.Contains("--REDIS_CONNECTION") 
                ? args[Array.IndexOf(args, "--REDIS_CONNECTION") + 1] 
                : Environment.GetEnvironmentVariable("REDIS_CONNECTION");

            MONGO_CONNECTION = args.Contains("--MONGO_CONNECTION")
                ? args[Array.IndexOf(args, "--MONGO_CONNECTION") + 1]
                : Environment.GetEnvironmentVariable("MONGO_CONNECTION");

            CACHE_DIRECTORY = args.Contains("--CACHE_DIRECTORY")
                ? args[Array.IndexOf(args, "--CACHE_DIRECTORY") + 1]
                : Environment.GetEnvironmentVariable("CACHE_DIRECTORY");

            IndexDictionary = JsonDataUtility.GetIndexDictionary(CACHE_DIRECTORY, usePresentationModels: true);

            var redisLoader = new RedisLoader(REDIS_CONNECTION);
            redisLoader.Load(IndexDictionary);

            var mongoLoader = new MongoLoader(MONGO_CONNECTION);
            mongoLoader.Load(IndexDictionary);

            Console.WriteLine("Done!");
        }
    }
}