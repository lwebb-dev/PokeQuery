using PokeApiNet;
using PokeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeCache
{
    public class Application
    {
        private readonly PokeApiClient client;
        private readonly string CACHE_DIRECTORY;

        public string FILE_EXTENSION => ".txt";
        public Dictionary<ResourceTypes, System.Type> ResourceTypeMap => new Dictionary<ResourceTypes, System.Type>
        {
            { ResourceTypes.Pokemon, typeof(Pokemon) },
            { ResourceTypes.Moves, typeof(Move) },
            { ResourceTypes.Items, typeof(Item) },
            { ResourceTypes.Types, typeof(PokeApiNet.Type) },
            { ResourceTypes.VersionGroups, typeof(VersionGroup) },
            { ResourceTypes.Generations, typeof(Generation) }
        };

        public Application()
        {
            this.client = new PokeApiClient();
            this.CACHE_DIRECTORY = Environment.GetEnvironmentVariable("CACHE_DIRECTORY");
        }

        public async Task RunAsync()
        {
            Directory.CreateDirectory(this.CACHE_DIRECTORY);

            MethodInfo writeFileMethod = typeof(Application).GetMethod(nameof(Application.WriteNamedResourceFile));

            foreach (KeyValuePair<ResourceTypes, System.Type> kvp in this.ResourceTypeMap)
            {
                string resourceName = Enum.GetName(typeof(ResourceTypes), kvp.Key).ToLower();

                if (File.Exists($"{this.CACHE_DIRECTORY}/{resourceName}{FILE_EXTENSION}"))
                {
                    Console.WriteLine($"{resourceName}{FILE_EXTENSION} exists, skipping...");
                    continue;
                }

                MethodInfo genericMethod = writeFileMethod.MakeGenericMethod(kvp.Value);
                object[] methodParams = new object[] { kvp.Key };
                Task task = (Task)genericMethod.Invoke(this, methodParams);
                await task.ConfigureAwait(false);
                PropertyInfo resultProperty = task.GetType().GetProperty("Result");
            }

            Console.WriteLine("Done!");
        }

        public async Task WriteNamedResourceFile<T>(ResourceTypes resourceType)
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
