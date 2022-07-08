from asyncio import subprocess
import os, subprocess

baseDir = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))
frontendDir = f"{baseDir}\\src\\frontend\\poke-query"
subprocess.check_call(f"cd {frontendDir} && npm run dev", shell=True)
