using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PokeLoader
{
    public static class JsonDataUtility
    {
        private static bool USE_PRESENTATION_MODELS;

        public static Dictionary<string, Dictionary<int, string>> GetIndexDictionary(string cacheDirectory, bool usePresentationModels)
        {
            USE_PRESENTATION_MODELS = usePresentationModels;
            var results = new Dictionary<string, Dictionary<int, string>>();

            foreach (string resourceDir in Directory.GetDirectories(cacheDirectory))
            {
                Uri resourceUri = new Uri(resourceDir);
                string index = resourceUri.Segments.Last();

                if (USE_PRESENTATION_MODELS && !IndexHelper.ValidIndexes.Contains(index))
                    continue;

                results.Add(index, GetKeyedJsons(resourceDir, index));
            }

            return results;
        }

        private static Dictionary<int, string> GetKeyedJsons(string resourceDir, string index)
        {
            var results = new Dictionary<int, string>();

            foreach (string itemDir in Directory.GetDirectories(resourceDir))
            {
                Uri itemUri = new Uri(itemDir);
                int id = int.Parse(itemUri.Segments.Last());
                string[] files = Directory.GetFiles(itemDir);
                string rawJson = File.ReadAllText(files.SingleOrDefault(x => x.Contains("index.json")));

                results.Add(id, USE_PRESENTATION_MODELS && IndexHelper.GetPresentationJsonFromIndex(index, rawJson, out string json) ? json : rawJson);
            }

            return results;
        }
    }
}
