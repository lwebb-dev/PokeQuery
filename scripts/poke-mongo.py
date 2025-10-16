import sys, os, subprocess

def help():
    print("Usage: python ./poke-mongo.py [ up | down ] [ --new-volume ]")
    sys.exit()

if (len(sys.argv) < 2 or len(sys.argv) > 3):
    help()

hasProfile = False

for profile in [ "up", "down" ]:
    if (profile == sys.argv[1].lower()):
        hasProfile = True
        break

if (hasProfile == False):
    help()

volumeName = "poke-mongo"
repoRoot = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))

if (len(sys.argv) == 3 and sys.argv[2].lower() == "--new-volume"):

    if (os.path.exists(f"{repoRoot}/docker/{volumeName}") == False):
        os.mkdir(f"{repoRoot}/docker/{volumeName}")

    if volumeName in subprocess.run(f"docker volume rm {volumeName} --force", capture_output=True).stdout.decode("utf-8"):
        print(f"Volume {volumeName} deleted successfully.")

    if volumeName in subprocess.run(f"docker volume create {volumeName}", capture_output=True).stdout.decode("utf-8"):
        print(f"Volume {volumeName} created successfully.")

if (sys.argv[1].lower() == "up"):
    os.system("docker pull mongo")
    os.system(f"docker run --volume {volumeName}:/data/db --name {volumeName} -e MONGO_INITDB_ROOT_USERNAME=root -e MONGO_INITDB_ROOT_PASSWORD=example -p 27017:27017 -d mongo")
    os.system(f"docker pull mongo-express")
    os.system(f"docker run --name mongo-express --link {volumeName}:mongo -e ME_CONFIG_MONGODB_ADMINUSERNAME=root -e ME_CONFIG_MONGODB_ADMINPASSWORD=example -e ME_CONFIG_MONGODB_URL=mongodb://root:example@mongo:27017/ -e ME_CONFIG_BASICAUTH=false -p 8082:8081 -d mongo-express")

if (sys.argv[1].lower() == "down"):
    os.system(f"docker stop mongo-express 2>nul")
    os.system(f"docker rm mongo-express --force 2>nul")
    os.system(f"docker stop {volumeName} 2>nul")
    os.system(f"docker rm {volumeName} --force 2>nul")
