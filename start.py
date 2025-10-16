import sys, os, subprocess

def clear():
    os.system("cls" if os.name is "nt" else "clear")

def handleExit():
    try:
        sys.exit(0)
    except SystemExit:
        os._exit(0)


def buildEnvFiles(defaultParam):
    subprocess.check_call(f"python ./scripts/build-env-files.py {defaultParam}", shell=True)
    return

def stopRedis():
    subprocess.check_call("python ./scripts/poke-redis.py down", shell=True)
    return

def stopMongo():
    subprocess.check_call("python ./scripts/poke-mongo.py down", shell=True)
    return

def startRedis(newVolumeParam):
    stopRedis()
    subprocess.check_call(f"python ./scripts/poke-redis.py up {newVolumeParam}", shell=True)
    return

def startMongo(newVolumeParam):
    stopMongo()
    subprocess.check_call(f"python ./scripts/poke-mongo.py up {newVolumeParam}", shell=True)
    return

def startPokeQueryBackend():
    subprocess.check_call("start cmd.exe /k \"python ./scripts/run-poke-query.py\"", shell=True)
    return

def openFrontendAppInBrowser():
    os.system("start http://localhost:5173")
    return

def startPokeQueryFrontend():
    subprocess.check_call("start cmd.exe /k \"python ./scripts/run-frontend.py\"", shell=True)
    return

def openInVsCode():
    subprocess.check_call("python ./scripts/open-repo-vscode.py\"", shell=True)
    return

def openInVisualStudio():
    subprocess.Popen("python ./scripts/open-pokequery-solution.py", shell=True)
    return

def createPokeDataVolume():
    subprocess.Popen("python ./scripts/create-poke-data-volume.py", shell=True)
    return

def runPokeLoader():
    subprocess.check_call("python ./scripts/run-poke-loader.py", shell=True)
    return

def lowerInput(prompt):
    return input(prompt).lower()

def handleInput(input):
    displayOptions = True

    match input:
        case "1":
            createPokeDataVolume()
            buildEnvFiles("--use-default")
            startRedis("--new-volume")
            startMongo("--new-volume")
            runPokeLoader()
            startPokeQueryBackend()
            startPokeQueryFrontend()
            openFrontendAppInBrowser()
        case "2":
            createPokeDataVolume()
        case "3":
            buildEnvFiles("--use-default")
        case "4":
            buildEnvFiles("")
        case "5":
            startMongo("")
        case "6":
            startMongo("--new-volume")
        case "7":
            stopMongo()
        case "8":
            startRedis("")
        case "9":
            startRedis("--new-volume")
        case "10":
            stopRedis()
        case "11":
            runPokeLoader()
        case "12":
            startPokeQueryBackend()
        case "13":
            startPokeQueryFrontend()
        case "14":
            openFrontendAppInBrowser()
        case "15":
            openInVsCode()
        case "16":
            openInVisualStudio()
        case "x":
            handleExit()
        case _:
            print(f"Unknown Input Value: {input}")
            displayOptions = False

    return mainMenu(displayOptions)

def mainMenu(displayOptions):

    if (displayOptions):
        print("Please select from the following options [1-16, X] or Ctrl + C to Exit.")
        print("1. Start Everything From Scratch")
        print("2. Create PokeData Volume")
        print("3. Build/Rebuild .env Files With Default Values")
        print("4. Build/Rebuild .env Files With User Defined Values")
        print("5. Start Mongo Container")
        print("6. Start Mongo Container With New Volume")
        print("7. Stop Mongo Container")
        print("8. Start Redis Container")
        print("9. Start Redis Container With New Volume")
        print("10. Stop Redis Container")
        print("11. Run PokeLoader (Load data into Redis/Mongo)")
        print("12. Start PokeQuery Backend")
        print("13. Start poke-query Frontend")
        print("14. Open Frontend App In Browser")
        print("15. Open Repo in VS Code")
        print("16. Open PokeQuery.sln")
        print("X. Exit/Quit")

    handleInput(lowerInput("Input Value: "))
    return

def main():
    clear()
    mainMenu(True)

if __name__ == '__main__':
    try:
        main()
    except KeyboardInterrupt:
        handleExit()
