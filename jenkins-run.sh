#! /usr/bin/env bash
echo "run docker."
docker network rm cfa || true
docker-compose down || true
docker-compose up -d
