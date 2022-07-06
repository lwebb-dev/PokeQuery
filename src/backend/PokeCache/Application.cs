using PokeApiNet;
using PokeLib;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeCache
{
    public class Application
    {
        private readonly PokeApiClient client;
        private readonly string CACHE_DIRECTORY;

        public Application()
        {
            this.client = new PokeApiClient();
            this.CACHE_DIRECTORY = Environment.GetEnvironmentVariable("CACHE_DIRECTORY");
        }

        public async Task RunAsync()
        {
            Directory.CreateDirectory(this.CACHE_DIRECTORY);

            if (!File.Exists($"{this.CACHE_DIRECTORY}/pokemon.txt"))
                await this.WriteNamedResourceFile<Pokemon>("pokemon");
            else
                Console.WriteLine("pokemon.txt exists, skipping...");

            if (!File.Exists($"{this.CACHE_DIRECTORY}/moves.txt"))
                await this.WriteNamedResourceFile<Move>("moves");
            else
                Console.WriteLine("moves.txt exists, skipping...");

            if (!File.Exists($"{this.CACHE_DIRECTORY}/items.txt"))
                await this.WriteNamedResourceFile<Item>("items");
            else
                Console.WriteLine("items.txt exists, skipping...");

            Console.WriteLine("Done!");
        }

        private async Task WriteNamedResourceFile<T>(string resourceType)
            where T : NamedApiResource
        {
            DirectoryInfo resourceDir = Directory.CreateDirectory($"{this.CACHE_DIRECTORY}/{resourceType}");
            string absoluteFilename = $"{resourceDir.FullName}.txt";
            NamedApiResourceList<T> namedResources = await this.client.GetNamedResourcePageAsync<T>(100000, 0);

            using StreamWriter file = new(absoluteFilename);

            foreach (var nr in namedResources.Results)
            {
                string json = JsonSerializer.Serialize(new CachedResource
                {
                    Name = nr.Name,
                    ResourceType = resourceType,
                    Url = nr.Url,
                    Json = string.Empty
                });

                await file.WriteLineAsync(json);

                if (!File.Exists($"{resourceDir}/{nr.Name}.txt"))
                    await WriteBlankJsonTextFile($"{resourceDir}/{nr.Name}.txt");
            }

            await file.DisposeAsync();
        }

        private static async Task WriteBlankJsonTextFile(string absoluteFilePath)
        {
            using StreamWriter file = new(absoluteFilePath);
            await file.WriteAsync(string.Empty);
            await file.DisposeAsync();
        }
    }
}
