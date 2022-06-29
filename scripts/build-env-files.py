import os

repoRoot = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))
projects = [ "src/backend/PokeCache", "src/backend/PokeConsole" ]
envVars = [ "CACHE_DIRECTORY", "MAX_RESULT_SIZE" ]
values = []

os.system("cls" if os.name == "nt" else "clear")

for x in envVars:
    value = input(f"{x}: ")
    value = f"{x}={value}\n"
    values.append(value)

for project in projects:
    filepath = f"{repoRoot}/{project}/.env"
    file = open(filepath, "w")
    file.writelines(values)
    file.close()
    print(f"successfully created {filepath}")
