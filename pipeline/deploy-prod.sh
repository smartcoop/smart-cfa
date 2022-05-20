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
    ../../src/UserAdmin/src/Smart.FA.Catalog.Web/appsettings.Production.json > ../../src/UserAdmin/src/Smart.FA.Catalog.Web/appsettings.Production.tmp.json

mv ../../src/UserAdmin/src/Smart.FA.Catalog.Web/appsettings.Production.tmp.json ../../src/UserAdmin/src/Smart.FA.Catalog.Web/appsettings.Production.json

docker build \
  --build-arg Environment="Production" \
  -f "../docker/kerberos/Web.krb5.Dockerfile" \
  -t "cfa_production_api" \
  .

docker build \
  --build-arg Environment="Production" \
  -f "../docker/kerberos/Showcase.krb5.Dockerfile" \
  -t "cfa-production-public" \
  .

docker stop cfa_production_api || true
docker rm cfa_production_api || true

docker stop cfa_production_public || true
docker rm cfa_production_public || true

docker network create --driver bridge --subnet 172.22.100.0/24 --attachable cfa || true

echo "RUN DOCKER"
docker run -d \
  --name cfa_production_api \
  --env Environment="Production" \
  --env-file ./.env \
  --volume $(pwd)/ktutil/files/krb5.conf:/etc/krb5.conf \
  --volume $(pwd)/ktutil/files/${DB_USER}.keytab:/app/${DB_USER}.keytab \
  --network=cfa \
  -p "8087:80" \
  cfa_production_api

  docker run -d \
    --name cfa_production_public \
    --env Environment="Production" \
    --env-file ./.env \
    --volume $(pwd)/ktutil/files/krb5.conf:/etc/krb5.conf \
    --volume $(pwd)/ktutil/files/${DB_USER}.keytab:/app/${DB_USER}.keytab \
    --network=cfa \
    -p "8086:80" \
    cfa-production-public
