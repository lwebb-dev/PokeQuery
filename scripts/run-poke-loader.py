import os, subprocess

repoRoot = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))
loaderDir = f"{repoRoot}\\src\\backend\\PokeLoader"

print("Building PokeLoader image...")
subprocess.check_call(f"docker build -t pokequery-loader {loaderDir}", shell=True)

print("Running PokeLoader container...")
subprocess.check_call(
    "docker run --rm "
    "--volume poke-data:/poke-data "
    "-e CACHE_DIRECTORY=/poke-data "
    "-e REDIS_CONNECTION=host.docker.internal:6379 "
    "-e MONGO_CONNECTION=mongodb://root:example@host.docker.internal:27017/ "
    "pokequery-loader",
    shell=True
)

print("PokeLoader completed!")
