import os, subprocess

baseDir = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))
pokeQueryDir = f"{baseDir}\\src\\backend-legacy\\PokeQuery"
subprocess.check_call(f"cd {pokeQueryDir} && dotnet run --project .\PokeQuery.csproj", shell=True)
