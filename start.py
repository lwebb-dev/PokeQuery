import os, subprocess

subprocess.check_call("python ./scripts/build-env-files.py --use-default", shell=True)
subprocess.check_call("python ./scripts/run-poke-cache.py", shell=True)
subprocess.check_call("python ./scripts/poke-redis.py up --new-volume", shell=True)
subprocess.check_call("start cmd.exe /k \"python ./scripts/run-poke-query.py\"", shell=True)
subprocess.check_call("start cmd.exe /k \"python ./scripts/run-frontend.py\"", shell=True)
os.system("start http://localhost:3000")