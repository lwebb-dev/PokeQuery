using System;
using System.IO;

namespace PokeConsole
{
    public class Program
    {
        public static void Main()
        {
            LoadEnv();
            new Application().RunAsync().GetAwaiter().GetResult();
        }

        private static void LoadEnv()
        {
            string filePath = $"{Directory.GetCurrentDirectory()}\\.env";

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Missing .env file.");

            foreach (string line in File.ReadAllLines(filePath))
            {
                string[] parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}