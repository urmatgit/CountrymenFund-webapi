#!/bin/bash
echo "start publish"
cd /projects/CountrymenFund-webapi/src/Host
dotnet publish -c Release -r linux-x64 -o published /projects/CountrymenFund-webapi/src/Host/Host.csproj
cd /projects/CountrymenFund-wasm/src/Host
dotnet publish -c Release -r linux-x64 -o published /projects/CountrymenFund-wasm/src/Host/Host.csproj
echo "stop kestrel-fundWebApi.service"
systemctl stop kestrel-fundWebApi.service
echo "stop fundWasm.service"
systemctl stop fundWasm.service
echo "cp -rT /projects/CountrymenFund-webapi/src/Host/published/ /var/www/fundapi/"
cp -rT /projects/CountrymenFund-webapi/src/Host/published/ /var/www/fundapi/
echo "cp -rT /projects/CountrymenFund-wasm/src/Host/published/ /var/www/fundwasm/"
cp -rT /projects/CountrymenFund-wasm/src/Host/published/ /var/www/fundwasm/
echo "start kestrel-fundWebApi.service"
systemctl start kestrel-fundWebApi.service
echo "start fundWasm.service"
systemctl start fundWasm.service
echo "restart nginx"
systemctl restart nginx