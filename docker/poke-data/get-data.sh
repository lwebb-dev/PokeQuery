#!/usr/bin/env bash

git clone --depth=1 https://github.com/PokeAPI/api-data.git

if [ ! -d ${REPO_DATA} ]; then
    echo "ERROR: ${REPO_DATA} directory does not exist"
    exit 1
fi

echo "creating /poke-data directory..."
mkdir -p /poke-data

echo "copying data from ${REPO_DATA} into /poke-data..."
cp -R ${REPO_DATA}/* /poke-data

echo "deleting /api-data..."
rm -rf api-data

echo "Done!"
