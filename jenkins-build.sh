#! /usr/bin/env bash

echo "Clean images."

docker rm -f "cfa-datasource" || true
docker rm -f "cfa-api" || true

docker image rm "cfa-api" || true
docker image rm "cfa-datasource" || true

docker image prune -f

echo "build docker."

docker build  \
  -f Api.Dockerfile \
  -t cfa-api \
  .


docker build  \
  -f DB.Dockerfile \
  -t cfa-datasource \
  .
