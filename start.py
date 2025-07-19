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

def startRedis(newVolumeParam):
    stopRedis()
    subprocess.check_call(f"python ./scripts/poke-redis.py up {newVolumeParam}", shell=True)
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

def lowerInput(prompt):
    return input(prompt).lower()

def handleInput(input):
    displayOptions = True

    match input:
        case "1":
            createPokeDataVolume()
            buildEnvFiles("--use-default")
            startRedis("--new-volume")
            startPokeQueryBackend()
            startPokeQueryFrontend()
            openFrontendAppInBrowser()
        case "2":
            buildEnvFiles("--use-default")
        case "3":
            buildEnvFiles("")
        case "4":
            startRedis("")
        case "5":
            startRedis("--new-volume")
        case "6":
            stopRedis()
        case "7":
            startPokeQueryBackend()
        case "8":
            startPokeQueryFrontend()
        case "9":
            openFrontendAppInBrowser()
        case "10":
            openInVsCode()
        case "11":
            openInVisualStudio()
        case "12":
            createPokeDataVolume()
        case "x":
            handleExit()
        case _:
            print(f"Unknown Input Value: {input}")
            displayOptions = False

    return mainMenu(displayOptions)

def mainMenu(displayOptions):

    if (displayOptions):
        print("Please select from the following options [1-12, X] or Ctrl + C to Exit.")
        print("1. Start Everything From Scratch")
        print("2. Build/Rebuild .env Files With Default Values")
        print("3. Build/Rebuild .env Files With User Defined Values")
        print("4. Start Redis Container")
        print("5. Start Redis Container With New Volume")
        print("6. Stop Redis Container")
        print("7. Start PokeQuery Backend")
        print("8. Start poke-query Frontend")
        print("9. Open Frontend App In Browser")
        print("10. Open Repo in VS Code")
        print("11. Open PokeQuery.sln")
        print("12. Create PokeData Volume")
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
