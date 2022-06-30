using PokeApiNet;
using PokeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeConsole
{
    public class Application
    {
        private readonly PokeApiClient client;
        private readonly string CACHE_DIRECTORY;
        private readonly byte MAX_RESULT_SIZE;

        public List<CachedResource> Cache { get; set; }
        public Application()
        {
            this.client = new PokeApiClient();
            this.Cache = new List<CachedResource>();
            this.CACHE_DIRECTORY = Environment.GetEnvironmentVariable("CACHE_DIRECTORY");
            this.MAX_RESULT_SIZE = byte.Parse(Environment.GetEnvironmentVariable("MAX_RESULT_SIZE"));
        }

        public async Task RunAsync()
        {
            this.Cache.AddRange(await this.LoadCachedResourceAsync("pokemon.txt"));
            this.Cache.AddRange(await this.LoadCachedResourceAsync("moves.txt"));
            this.Cache.AddRange(await this.LoadCachedResourceAsync("items.txt"));
            Console.WriteLine($"Cache Size: {this.Cache.Count}");

            string query = string.Empty;

            while (query.Length < 3)
            {
                Console.Write("Enter Query: ");
                query = Console.ReadLine();
            }

            string sanitizedQuery = query.ToLower().Replace(' ', '-');
            string[] queryValues = sanitizedQuery.Split(' ');
            IEnumerable<CachedResource> queryResult = this.Cache
                .Where(x => x.Name.ContainsAny(queryValues))
                //.Where(x => x.Name.IndexOfMany(queryValues) < 4)
                .OrderBy(x => x.Name.IndexOfMany(queryValues))
                .Take(MAX_RESULT_SIZE);

            if (queryResult.Count() == 0)
                return;

            //if (queryResult.First().Name.ToLower().Replace(' ', '-') == sanitizedQuery)
            //{
            //    queryResult = new List<CachedResource>
            //    {
            //        queryResult.FirstOrDefault(x => x.Name.Contains(sanitizedQuery))
            //    };
            //}

            foreach (var item in queryResult)
            {
                if (string.IsNullOrEmpty(item.Json))
                {
                    switch (item.ResourceType)
                    {
                        case "pokemon":
                            item.Json = await this.GetPokeApiJsonResult<Pokemon>(item);
                            break;
                        case "items":
                            item.Json = await this.GetPokeApiJsonResult<Item>(item);
                            break;
                        case "moves":
                            item.Json = await this.GetPokeApiJsonResult<Move>(item);
                            break;
                    }
                }

                Console.WriteLine(item.Name);
                Console.WriteLine(item.ResourceType);
                Console.WriteLine(item.Url);
                Console.WriteLine(item.Json);
            }
        }

        private async Task<List<CachedResource>> LoadCachedResourceAsync(string filename)
        {
            var result = new List<CachedResource>();
            string[] lines;
            string fileDirectory = $"{this.CACHE_DIRECTORY}\\{filename}";

            if (!File.Exists(fileDirectory))
                return result;

            lines = await File.ReadAllLinesAsync(fileDirectory);

            foreach (string line in lines)
            {
                result.Add(JsonSerializer.Deserialize<CachedResource>(line));
            }

            return result;
        }

        private async Task<string> GetPokeApiJsonResult<T>(CachedResource cachedResource)
            where T : NamedApiResource
        {
            string[] splitUri = cachedResource.Url.Split("/");
            int id = int.Parse(splitUri[splitUri.Length - 2]);
            T result = await this.client.GetResourceAsync<T>(id);
            return JsonSerializer.Serialize(result);
        }
    }
}
