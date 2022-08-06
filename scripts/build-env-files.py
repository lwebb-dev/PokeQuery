import sys, os

repoRoot = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))
projects = [ "src/backend-legacy/PokeCache", "src/backend-legacy/PokeQuery", "src/frontend/poke-query" ]
envVars = [ "MAX_RESULT_SIZE", "API_BASE_URI", "REDIS_CONNECTION_STRING" ]
defaultValues = [ 12, "http://localhost:5244", "localhost:6379" ]
useDefaultValues = len(sys.argv) == 2 and sys.argv[1].lower() == "--use-default"
values = []

def setValues():
    if (useDefaultValues):
        for i in range(len(envVars)):
            values.append(f"{envVars[i]}={defaultValues[i]}\n")
        return
    
    for i in range(len(envVars)):
        value = input(f"{envVars[i]} ({defaultValues[i]}): ")

        if (value == ""):
            value = defaultValues[i]

        value = f"{envVars[i]}={value}\n"
        values.append(value)

setValues()

for project in projects:
    filepath = f"{repoRoot}/{project}/.env"
    file = open(filepath, "w")
    file.writelines(values)
    file.close()
    print(f"successfully created {filepath}")
