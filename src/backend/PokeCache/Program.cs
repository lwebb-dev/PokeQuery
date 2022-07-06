using PokeLib.Utilities;

namespace PokeCache
{
    public class Program
    {
        public static void Main()
        {
            EnvironmentVarUtility.LoadEnvironmentVariablesFromDotEnv();
            new Application().RunAsync().GetAwaiter().GetResult();
        }
    }
}