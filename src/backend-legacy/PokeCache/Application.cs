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
        public Dictionary<NamedResourceTypes, System.Type> NamedResourceTypeMap => new Dictionary<NamedResourceTypes, System.Type>
        {
            { NamedResourceTypes.Pokemon, typeof(Pokemon) },
            { NamedResourceTypes.Moves, typeof(Move) },
            { NamedResourceTypes.Items, typeof(Item) },
            { NamedResourceTypes.Types, typeof(PokeApiNet.Type) },
            { NamedResourceTypes.VersionGroups, typeof(VersionGroup) },
            { NamedResourceTypes.Generations, typeof(Generation) }
        };

        public Dictionary<ResourceTypes, System.Type> ResourceTypeMap => new Dictionary<ResourceTypes, System.Type>
        {
            { ResourceTypes.Machines, typeof(Machine) }
        };

        public Application()
        {
            this.client = new PokeApiClient();
            this.CACHE_DIRECTORY = Environment.GetEnvironmentVariable("CACHE_DIRECTORY");
        }

        public async Task RunAsync()
        {
            Directory.CreateDirectory(this.CACHE_DIRECTORY);

            await HandleNamedResourceFiles();
            await HandleApiResourceFiles();

            Console.WriteLine("Done!");
        }

        public async Task HandleNamedResourceFiles()
        {
            MethodInfo writeFileMethod = typeof(Application).GetMethod(nameof(Application.WriteNamedResourceFileAsync));

            foreach (KeyValuePair<NamedResourceTypes, System.Type> kvp in this.NamedResourceTypeMap)
            {
                string resourceName = Enum.GetName(typeof(NamedResourceTypes), kvp.Key).ToLower();

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
        }

        // TODO: Abstract out duplicate Method/TypeMap logic into separate method using reflection
        public async Task HandleApiResourceFiles()
        {
            MethodInfo writeFileMethod = typeof(Application).GetMethod(nameof(Application.WriteResourceFileAsync));

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

        }

        public async Task WriteNamedResourceFileAsync<T>(NamedResourceTypes namedResourceType)
            where T : NamedApiResource
        {
            DirectoryInfo resourceDir = Directory.CreateDirectory($"{this.CACHE_DIRECTORY}/{Enum.GetName(typeof(NamedResourceTypes), namedResourceType).ToLower()}");
            string absoluteFilename = $"{resourceDir.FullName}.txt";
            NamedApiResourceList<T> namedResources = await this.client.GetNamedResourcePageAsync<T>(100000, 0);

            using StreamWriter file = new(absoluteFilename);

            foreach (var nr in namedResources.Results)
            {
                string json = JsonSerializer.Serialize(new NamedCachedResource
                {
                    Name = nr.Name,
                    NamedResourceType = namedResourceType,
                    Url = nr.Url,
                    Json = string.Empty
                });

                await file.WriteLineAsync(json);

                if (!File.Exists($"{resourceDir}/{nr.Name}{FILE_EXTENSION}"))
                    await WriteBlankJsonTextFile($"{resourceDir}/{nr.Name}{FILE_EXTENSION}");
            }

            await file.DisposeAsync();
        }

        public async Task WriteResourceFileAsync<T>(ResourceTypes resourceType)
            where T : ApiResource
        {
            string resourceName = Enum.GetName(typeof(ResourceTypes), resourceType).ToLower();
            DirectoryInfo resourceDir = Directory.CreateDirectory($"{this.CACHE_DIRECTORY}/{resourceName}");
            string absoluteFilename = $"{resourceDir.FullName}.txt";
            ApiResourceList<T> resources = await this.client.GetApiResourcePageAsync<T>(100000, 0);

            using StreamWriter file = new(absoluteFilename);

            foreach (ApiResource<T> resource in resources.Results)
            {
                CachedResource resourceObject = new CachedResource
                {
                    ResourceType = resourceType,
                    Url = resource.Url,
                    Json = string.Empty
                };

                await file.WriteLineAsync(JsonSerializer.Serialize(resourceObject));
                string absoluteFilePath = $"{resourceDir}/{resourceName}_{resourceObject.Id}{FILE_EXTENSION}";

                if (!File.Exists(absoluteFilePath))
                    await WriteBlankJsonTextFile(absoluteFilePath);
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
