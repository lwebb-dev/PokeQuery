using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PokeLib.Utilities
{
    public static class EnvironmentVarUtility
    {
        public static void LoadEnvironmentVariablesFromDotEnv()
        {
            string filePath = $"{Directory.GetCurrentDirectory()}\\.env";

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Missing .env file.");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                throw new DirectoryNotFoundException("Unsupported OSPlatform");

            foreach (string line in File.ReadAllLines(filePath))
            {
                string[] parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                // will be set later
                if (parts[0] == "CACHE_DIRECTORY")
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }

            // CACHE_DIRECTORY
            string cacheDirectory = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PokeQuery")
                : "~/../usr/bin/poke-query";

            Environment.SetEnvironmentVariable("CACHE_DIRECTORY", cacheDirectory);

        }
    }
}
