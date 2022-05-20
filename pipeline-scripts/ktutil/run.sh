
#! /usr/bin/env bash
docker build -t ktuil .
docker run -v $(pwd)/files:/files --env-file ./.env  ktuil:latest