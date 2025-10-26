# PokeQuery

Poc stack involving caching resources from [PokeApi](https://pokeapi.co/) and running simple queries against it using .NET 6.0, MongoDB, and React as the tech stack.

## Setup

In the interest of being OS agnostic, local dev setup scripts are written in Python.

First, make sure the following are installed:

1. [Python](https://www.python.org/) 
2. [.NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
3. [Docker](https://docs.docker.com/engine/install/)

From the repository root directory, run `python start.py` and select the first (1) option from the menu. This will:

1. Run a bash script in a docker container to cache bulk data from PokeApi repo to a dedicated poke-data volume.
2. Build .env files for each project using the default values.
3. Start separate Redis and MongoDB docker containers with empty volumes.
4. Run PokeLoader in a docker container using the poke-data volume, which will then mount the data into both the Redis Cache and MongoDB Database.
5. Start the PokeQuery backend Rest API project.
6. Start the poke-query frontend React app.
7. Open the web app in the default web browser (`http://localhost:5173`).
