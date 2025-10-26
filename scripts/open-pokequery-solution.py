import os, subprocess

# This assumes Visual Studio is already installed
baseDir = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))
backendDir = rf"{baseDir}\src\backend"
subprocess.check_call(rf"cd {backendDir} && .\PokeQuery.sln", shell=True)
