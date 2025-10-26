import os, subprocess

baseDir = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))
pokeQueryDir = rf"{baseDir}\src\backend\PokeQuery"
subprocess.check_call(rf"cd {pokeQueryDir} && dotnet run --project .\PokeQuery.csproj", shell=True)
