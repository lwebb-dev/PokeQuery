import os

repoRoot = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))
projects = [ "src/backend/PokeCache", "src/backend/PokeQuery", "src/frontend/poke-query" ]
envVars = [ "MAX_RESULT_SIZE", "API_BASE_URI", "REDIS_CONNECTION_STRING" ]
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
