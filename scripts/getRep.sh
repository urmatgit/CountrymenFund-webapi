#!/bin/bash
echo "start get from git"
cd ~/projects/CountymenFund-wasm/
git pull origin
cd ~/projects/CountrymenFund-webapi/
git pull origin
docker-compose down
docker-compose up

