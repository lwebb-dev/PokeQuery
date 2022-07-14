# PokeQuery

A simple proof of concept involving caching resources from [PokeApi](https://pokeapi.co/) and running simple queries against it using .NET 6.0 as the core technology.

## Setup

In the interest of being OS agnostic, backend projects are written in .NET 6.0, while development setup scripts are written using Python.

First, make sure the following are installed correctly:

1. [Python](https://www.python.org/) 
2. [.NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
3. [Docker](https://docs.docker.com/engine/install/)

From the repository root directory, simply run `python start.py` and select the first (1) option from the menu. This will:

1. Build .env files for each project using the default values.
2. Run PokeCache, which will cache all the Named Resources in AppData from PokeApi for pokemon, items, and moves. These Named Resources act as the indexes for future queries.
3. Start the Redis Docker Container with a dedicated volume. This will act as our data warehouse for the Named Resource and Json data that is being cached in AppData.
4. Start the PokeQuery backend Rest API project.
5. Start the poke-query frontend Svelte app.
6. Open the web app in the default web browser (currently set at `http://localhost:3000`).
