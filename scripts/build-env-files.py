import sys, os

repoRoot = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))
projects = [ "src/backend/PokeQuery", "src/frontend/poke-query" ]
envVars = [ "MAX_RESULT_SIZE", "API_BASE_URI", "REDIS_CONNECTION", "MONGO_CONNECTION" ]
defaultValues = [ 10, "http://localhost:5112", "localhost:6379", "mongodb://root:example@mongo:27017/" ]
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
        # If frontend, prefix envVars with VITE_
        if project == "src/frontend/poke-query":
            vite_values = []
            for i in range(len(envVars)):
                # Add VITE_ prefix to variable name
                var = f"VITE_{envVars[i]}"
                # Get value from values (already formatted as VAR=VAL\n)
                val = values[i].split("=", 1)[1]
                vite_values.append(f"{var}={val}")
            file = open(filepath, "w")
            file.writelines(vite_values)
            file.close()
            print(f"successfully created {filepath}")
        else:
            file = open(filepath, "w")
            file.writelines(values)
            file.close()
            print(f"successfully created {filepath}")
