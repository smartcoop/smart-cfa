#! /usr/bin/env bash

set -e

echo "KTUTIL_USER=${DB_USER}" > .env
echo "KTUTIL_PASS=${DB_PASS}" >> .env
echo "user=${DB_USER}" >> .env


# generate myUserName.keytab
docker build -t ktuil ./ktutil
docker run -v $(pwd)/ktutil/files:/files --env-file ./.env  ktuil:latest

# change permission myUserName.keytab
sudo chmod 777 $(pwd)/ktutil/files/$DB_USER.keytab

sed -e "s/{minio_access-key}/$MINIO_ACCESS_KEY/" \
    -e "s/{minio_secret-key}/$MINIO_SECRET_KEY/" \
    ./src/UserAdmin/src/Smart.FA.Catalog.Web/appsettings.PreProduction.json > ./src/UserAdmin/src/Smart.FA.Catalog.Web/appsettings.PreProduction.tmp.json

mv ./src/UserAdmin/src/Smart.FA.Catalog.Web/appsettings.PreProduction.tmp.json ./src/UserAdmin/src/Smart.FA.Catalog.Web/appsettings.PreProduction.json

docker build \
  --build-arg Environment="PreProduction" \
  -f "Web.Dockerfile" \
  -t "cfa-staging-api" \
  .

docker stop cfa_staging_api || true
docker rm cfa_staging_api || true


docker run -d \
  --name cfa_staging_api \
  --env Environment="PreProduction" \
  --env-file ./.env \
  --volume $(pwd)/ktutil/files/krb5.conf:/etc/krb5.conf \
  --volume $(pwd)/ktutil/files/smartserver_STG.keytab:/app/smartserver_STG.keytab \
  -p "6000:80" \
  cfa-staging-api
