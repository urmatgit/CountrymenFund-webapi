#!/bin/bash
echo "start publish"
dotnet publish /projects/CountrymenFund-webapi/src/Host/Host.csproj -с Release -r linux-x64 -o published
dotnet publish /projects/CountrymenFund-wasm/src/Host/Host.csproj -с Release -r linux-x64 -o published
systemctl stop kestrel-fundWebApi.service
systemctl stop fundWasm.service
cp -rT /projects/CountrymenFund-webapi/src/Host/published/ /var/www/fundapi/
cp -rT /projects/CountrymenFund-wasm/src/Host/published/ /var/www/fundwasm/
systemctl start kestrel-fundWebApi.service
systemctl start fundWasm.service
systemctl restart nginxcd