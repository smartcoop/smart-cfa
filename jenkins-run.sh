#! /usr/bin/env bash
echo "run docker."
docker-compose down || true
docker-compose up
