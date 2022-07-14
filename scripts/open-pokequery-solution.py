import os, subprocess

# This assumes Visual Studio is already installed
baseDir = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))
backendDir = f"{baseDir}\\src\\backend"
subprocess.check_call(f"cd {backendDir} && .\PokeQuery.sln", shell=True)
