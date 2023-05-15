#!/bin/bash
echo "start get from git"
cd /project/CountymenFund-wasm/
git pull git@github.com:urmatgit/CountrymenFund-wasm.git
cd /project/CountrymenFund-webapi/
git reset --hard origin
git pull git@github.com:urmatgit/CountrymenFund-webapi.git
cd scripts
chmod +x publish.sh
./publish.sh