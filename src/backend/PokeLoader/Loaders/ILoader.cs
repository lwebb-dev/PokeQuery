using System.Collections.Generic;

namespace PokeLoader.Loaders;

public interface ILoader
{
    void Load(Dictionary<string, Dictionary<int, string>> indexDictionary);
}
