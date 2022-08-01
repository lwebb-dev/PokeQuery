import os

repoRoot = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))

print("Building poke-data image...")
os.system(f"docker build -t poke-data {repoRoot}/docker/poke-data")

print("Running poke-data container...")
os.system("docker run --volume poke-data:/poke-data -d -t poke-data")

print("create-poke-data-volume Done!")
