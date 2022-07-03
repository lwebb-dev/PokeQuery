import sys, os, subprocess

def help():
    print("Usage: python ./poke-redis.py [ up | down ] [ --new-volume ]")
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

volumeName = "poke-redis"
repoRoot = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))

if (len(sys.argv) == 3 and sys.argv[2].lower() == "--new-volume"):

    if (os.path.exists(f"{repoRoot}\docker") == False):
        os.mkdir(f"{repoRoot}\docker")

    if (os.path.exists(f"{repoRoot}\docker\{volumeName}") == False):
        os.mkdir(f"{repoRoot}\docker\{volumeName}")

    if volumeName in subprocess.run(f"docker volume rm {volumeName} --force", capture_output=True).stdout.decode("utf-8"):
        print(f"Volume {volumeName} deleted successfully.")

    if volumeName in subprocess.run(f"docker volume create {volumeName}", capture_output=True).stdout.decode("utf-8"):
        print(f"Volume {volumeName} created successfully.")

if (sys.argv[1].lower() == "up"):
    os.system("docker pull redis")
    os.system(f"docker run --volume {volumeName}:/data --name {volumeName} -d redis redis-server --save 60 1 --loglevel warning")

if (sys.argv[1].lower() == "down"):
    os.system(f"docker stop {volumeName}")
    os.system(f"docker rm {volumeName} --force")