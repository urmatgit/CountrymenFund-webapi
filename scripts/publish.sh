#!/bin/bash
echo "start publish"
cd /projects/CountrymenFund-webapi/src/Host
rm -r bin
rm -r obj
rm -r published
rm -r /projects/CountrymenFund-webapi/src/Core/Application/obj
rm -r /projects/CountrymenFund-webapi/src/Core/Application/bin
rm -r /projects/CountrymenFund-webapi/src/Core/Domain/obj
rm -r /projects/CountrymenFund-webapi/src/Core/Domain/bin
rm -r /projects/CountrymenFund-webapi/src/Core/Shared/obj
rm -r /projects/CountrymenFund-webapi/src/Core/Shared/bin

rm -r /projects/CountrymenFund-webapi/src/Infrastructure/obj
rm -r /projects/CountrymenFund-webapi/src/Infrastructure/bin

dotnet publish -c Release -r linux-x64 -o published /projects/CountrymenFund-webapi/src/Host/Host.csproj

cd /projects/CountrymenFund-wasm/src/Host
rm -r /projects/CountrymenFund-wasm/src/Host/bin
rm -r /projects/CountrymenFund-wasm/src/Host/obj
rm -r /projects/CountrymenFund-wasm/src/Host/published

rm -r /projects/CountrymenFund-wasm/src/Client/bin
rm -r /projects/CountrymenFund-wasm/src/Client/obj

rm -r /projects/CountrymenFund-wasm/src/Client.Infrastructure/bin
rm -r /projects/CountrymenFund-wasm/src/Client.Infrastructure/obj

rm -r /projects/CountrymenFund-wasm/src/Shared/bin
rm -r /projects/CountrymenFund-wasm/src/Shared/obj


dotnet publish -c Release -r linux-x64 --no-self-contained -p:PublishTrimmed=false -o published /projects/CountrymenFund-wasm/src/Host/Host.csproj
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