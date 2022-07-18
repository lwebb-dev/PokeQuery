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

        public string FILE_EXTENSION => ".txt";

        public Application()
        {
            this.client = new PokeApiClient();
            this.CACHE_DIRECTORY = Environment.GetEnvironmentVariable("CACHE_DIRECTORY");
        }

        public async Task RunAsync()
        {
            Directory.CreateDirectory(this.CACHE_DIRECTORY);

            if (!File.Exists($"{this.CACHE_DIRECTORY}/{Enum.GetName(typeof(ResourceTypes), ResourceTypes.Pokemon)}{FILE_EXTENSION}"))
                await this.WriteNamedResourceFile<Pokemon>(ResourceTypes.Pokemon);
            else
                Console.WriteLine("pokemon.txt exists, skipping...");

            if (!File.Exists($"{this.CACHE_DIRECTORY}/{Enum.GetName(typeof(ResourceTypes), ResourceTypes.Moves)}{FILE_EXTENSION}"))
                await this.WriteNamedResourceFile<Move>(ResourceTypes.Moves);
            else
                Console.WriteLine("moves.txt exists, skipping...");

            if (!File.Exists($"{this.CACHE_DIRECTORY}/{Enum.GetName(typeof(ResourceTypes), ResourceTypes.Items)}{FILE_EXTENSION}"))
                await this.WriteNamedResourceFile<Item>(ResourceTypes.Items);
            else
                Console.WriteLine("items.txt exists, skipping...");

            if (!File.Exists($"{this.CACHE_DIRECTORY}/{Enum.GetName(typeof(ResourceTypes), ResourceTypes.Types)}{FILE_EXTENSION}"))
                await this.WriteNamedResourceFile<PokeApiNet.Type>(ResourceTypes.Types);
            else
                Console.WriteLine("types.txt exists, skipping...");

            if (!File.Exists($"{this.CACHE_DIRECTORY}/{Enum.GetName(typeof(ResourceTypes), ResourceTypes.VersionGroups)}{FILE_EXTENSION}"))
                await this.WriteNamedResourceFile<VersionGroup>(ResourceTypes.VersionGroups);
            else
                Console.WriteLine("versiongroups.txt exists, skipping...");

            if (!File.Exists($"{this.CACHE_DIRECTORY}/{Enum.GetName(typeof(ResourceTypes), ResourceTypes.Generations)}{FILE_EXTENSION}"))
                await this.WriteNamedResourceFile<Generation>(ResourceTypes.Generations);
            else
                Console.WriteLine("generations.txt exists, skipping...");

            Console.WriteLine("Done!");
        }

        private async Task WriteNamedResourceFile<T>(ResourceTypes resourceType)
            where T : NamedApiResource
        {
            DirectoryInfo resourceDir = Directory.CreateDirectory($"{this.CACHE_DIRECTORY}/{Enum.GetName(typeof(ResourceTypes), resourceType).ToLower()}");
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

                if (!File.Exists($"{resourceDir}/{nr.Name}{FILE_EXTENSION}"))
                    await WriteBlankJsonTextFile($"{resourceDir}/{nr.Name}{FILE_EXTENSION}");
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
