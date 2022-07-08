import os, subprocess

baseDir = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))
pokeCacheDir = f"{baseDir}\\src\\backend\\PokeCache"
subprocess.check_call(f"cd {pokeCacheDir} && dotnet run --project .\PokeCache.csproj", shell=True)