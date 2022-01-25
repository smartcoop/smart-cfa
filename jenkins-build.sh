#! /usr/bin/env bash
echo "build docker."

docker build  \
  -f Api.Dockerfile \
  -t cfa-api \
  .


docker build  \
  -f DB.Dockerfile \
  -t cfa-datasource \
  .
