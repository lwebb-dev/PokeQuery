using PokeLoader.PresentationModels;
using System.Text.Json;

namespace PokeLoader
{
    public static class IndexHelper
    {
        public static string[] ValidIndexes =
        {
            "pokemon",
            "item",
            "move",
            "nature",
            "type",
            "version-group",
            "machine",
            "pokemon-species"
        };
        public static bool GetPresentationJsonFromIndex(string index, string rawJson, out string json)
        {
            if (index == "pokemon")
            {
                json = ReserializeJsonFromPresentation<PokemonPresentation>(rawJson);
                return true;
            }

            if (index == "item")
            {
                json = ReserializeJsonFromPresentation<ItemPresentation>(rawJson);
                return true;
            }

            if (index == "move")
            {
                json = ReserializeJsonFromPresentation<MovePresentation>(rawJson);
                return true;
            }

            if (index == "nature")
            {
                json = ReserializeJsonFromPresentation<NaturePresentation>(rawJson);
                return true;
            }

            if (index == "type")
            {
                json = ReserializeJsonFromPresentation<TypePresentation>(rawJson);
                return true;
            }

            if (index == "version-group")
            {
                json = ReserializeJsonFromPresentation<VersionGroupPresentation>(rawJson);
                return true;
            }

            if (index == "machine")
            {
                json = ReserializeJsonFromPresentation<MachinePresentation>(rawJson);
                return true;
            }

            if (index == "pokemon-species")
            {
                json = ReserializeJsonFromPresentation<PokemonSpeciesPresentation>(rawJson);
                return true;
            }

            json = string.Empty;
            return false;
        }

        private static string ReserializeJsonFromPresentation<T>(string rawJson)
        {
            T presentation = JsonSerializer.Deserialize<T>(rawJson);
            string json = JsonSerializer.Serialize(presentation);
            return json;
        }
    }
}
