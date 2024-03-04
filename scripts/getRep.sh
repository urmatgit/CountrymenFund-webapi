#!/bin/bash
echo "start get from git"
cd /home/ubuntuuser/projects/CountrymenFund-wasm/
git pull origin
cd /home/ubuntuuser/projects/CountrymenFund-webapi/
git pull origin
docker-compose down
docker-compose up

