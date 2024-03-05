PASSWORD=Urmatbek82
if [ $# -ge 1 ]
  then
    PASSWORD=$1
fi

openssl req -x509 -out dev.crt -keyout dev.key -days 825 \
  -newkey rsa:2048 -nodes -sha256 \
  -subj '/CN=*81.94.159.193' -extensions EXT -config <( \
   printf "[dn]\nCN=*81.94.159.193\n[req]\ndistinguished_name = dn\n[EXT]\nsubjectAltName=DNS:81.94.159.193\nkeyUsage=digitalSignature\nextendedKeyUsage=serverAuth")

openssl pkcs12 -export -out aspnetapp1.pfx -inkey dev.key -in dev.crt -password pass:$PASSWORD