#!/bin/bash
echo "start publish"
cd /projects/CountrymenFund-webapi/src/Host
dotnet publish -c Release -r linux-x64 -o published /projects/CountrymenFund-webapi/src/Host/Host.csproj
cd /projects/CountrymenFund-wasm/src/Host
dotnet publish -c Release -r linux-x64 -o published /projects/CountrymenFund-wasm/src/Host/Host.csproj
systemctl stop kestrel-fundWebApi.service
systemctl stop fundWasm.service
cp -rT /projects/CountrymenFund-webapi/src/Host/published/ /var/www/fundapi/
cp -rT /projects/CountrymenFund-wasm/src/Host/published/ /var/www/fundwasm/
systemctl start kestrel-fundWebApi.service
systemctl start fundWasm.service
systemctl restart nginxsystena