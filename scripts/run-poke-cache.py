import os, subprocess

baseDir = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))
pokeCacheDir = rf"{baseDir}\src\backend-legacy\PokeCache"
subprocess.check_call(rf"cd {pokeCacheDir} && dotnet run --project .\PokeCache.csproj", shell=True)