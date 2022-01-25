#! /usr/bin/env bash
echo "run docker."

docker rm -f "cfa-datasource" || true
docker rm -f "cfa-api" || true


docker run -d \
  --name cfa-datasource \
  --restart=always \
  -p "1433:1433" \
  cfa-datasource


docker run -d \
  --name cfa-api \
  --restart=always \
  -p "8080:80" \
  cfa-api
