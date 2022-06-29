# PokeQuery

A simple proof of concept involving caching resources from [PokeApi](https://pokeapi.co/) and running simple queries against it using .NET 6.0 as the core technology.

## Setup

In the interest of being OS agnostic, backend projects are written in .NET 6.0, while development setup scripts are written using Python.

First, make sure the following are installed correctly:

1. [Python](https://www.python.org/) 
2. [.NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

Next, run `.\scripts\build-env-files.py` to build .env files for each app project:

    cd .\scripts
    python build-env-files.py

Finally, build & run PokeCache project to cache PokeApi Resources:

    cd .\src\backend\PokeCache
    dotnet build PokeCache.csproj
    cd .\bin\Debug\net6.0
    .\PokeCache.exe

You should now be able to build & run PokeConsole project and begin querying results against an in-memory cache of the resources pulled from PokeCache.