import os, subprocess

# This assumes VSCode is already installed and is in the current %PATH%
baseDir = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))
subprocess.check_call(f"cd {baseDir} && code .", shell=True)
