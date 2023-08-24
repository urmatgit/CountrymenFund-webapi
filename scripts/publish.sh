#!/bin/bash
echo "start publish"
cd /prj/CountrymenFund-webapi/src/Host
rm -r bin
rm -r obj
rm -r published
rm -r /prj/CountrymenFund-webapi/src/Core/Application/obj
rm -r /prj/CountrymenFund-webapi/src/Core/Application/bin
rm -r /prj/CountrymenFund-webapi/src/Core/Domain/obj
rm -r /prj/CountrymenFund-webapi/src/Core/Domain/bin
rm -r /prj/CountrymenFund-webapi/src/Core/Shared/obj
rm -r /prj/CountrymenFund-webapi/src/Core/Shared/bin

rm -r /prj/CountrymenFund-webapi/src/Infrastructure/obj
rm -r /prj/CountrymenFund-webapi/src/Infrastructure/bin

dotnet publish -c Release -r linux-x64 -o published /prj/CountrymenFund-webapi/src/Host/Host.csproj

cd /prj/CountrymenFund-wasm/src/Host
rm -r /prj/CountrymenFund-wasm/src/Host/bin
rm -r /prj/CountrymenFund-wasm/src/Host/obj
rm -r /prj/CountrymenFund-wasm/src/Host/published

rm -r /prj/CountrymenFund-wasm/src/Client/bin
rm -r /prj/CountrymenFund-wasm/src/Client/obj

rm -r /prj/CountrymenFund-wasm/src/Client.Infrastructure/bin
rm -r /prj/CountrymenFund-wasm/src/Client.Infrastructure/obj

rm -r /prj/CountrymenFund-wasm/src/Shared/bin
rm -r /prj/CountrymenFund-wasm/src/Shared/obj


dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishTrimmed=false -f net7.0 -o published /prj/CountrymenFund-wasm/src/Host/Host.csproj
echo "stop kestrel-fundWebApi.service"
systemctl stop kestrel-fundWebApi.service
echo "stop fundWasm.service"
systemctl stop fundWasm.service
echo "cp -rT /prj/CountrymenFund-webapi/src/Host/published/ /var/www/fundapi/"
cp -rT /prj/CountrymenFund-webapi/src/Host/published/ /var/www/fundapi/
echo "cp -rT /prj/CountrymenFund-wasm/src/Host/published/ /var/www/fundwasm/"
cp -rT /prj/CountrymenFund-wasm/src/Host/published/ /var/www/fundwasm/
echo "start kestrel-fundWebApi.service"
systemctl start kestrel-fundWebApi.service
echo "start fundWasm.service"
systemctl start fundWasm.service
echo "restart nginx"
systemctl restart nginx