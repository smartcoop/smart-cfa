#! /usr/bin/env bash

# This scripts is called by Jenkins whenever a branch is pushed to GitHub.

# Exit immediately from script if any command send back an error code
set -e

# Set-up docker container's name
DOCKER_NAME=$(echo cfa-"${BRANCH_NAME}" | sed -e 's/\(.*\)/\L\1/' -e 's/origin\/master/master/g' -e 's/origin\/feature\//feature-/g' -e 's/origin\/hotfix\//hotfix-/g' -e 's/feature\//feature-/g' -e 's/hotfix\//hotfix-/g')
DOCKER_HOSTNAME=${DOCKER_NAME}

# Exit without deploying if the branch's name use non-authorized character or if the branch is a PR branch
echo
if ! echo "${DOCKER_NAME}" |  grep -q "^[a-z][a-z0-9_-]*$"; then
  echo "The branch does not use only alphanumeric characters and dashes. Exiting.";
  exit 1
fi

# Create variables for tracking the metadata of the deploy
GIT_COMMIT="$(git rev-parse HEAD)"
GIT_COMMIT_SUBJECT="$(git log -n1 --format=format:"%s")"
GIT_COMMIT_DATE="$(git log -n1 --format=format:"%ci")"
GIT_AUTHOR_NAME="$(git log -n1 --format=format:"%aN")"
GIT_AUTHOR_EMAIL="$(git log -n1 --format=format:"%cE")"

echo "Starting jenkins-deploy.sh script...."
echo "DOCKER_NAME: ${DOCKER_NAME}"
echo "Branch name: ${DOCKER_HOSTNAME}"
echo "Domain: ${DOCKER_HOSTNAME}.smartcoop.dev"
echo "Git commit: ${GIT_COMMIT}"
echo "Git subject: ${GIT_COMMIT_SUBJECT}"
echo "Git date: ${GIT_COMMIT_DATE}"
echo "Git author name: ${GIT_AUTHOR_NAME}"
echo "Git author email: ${GIT_AUTHOR_EMAIL}"

{
    echo "DOCKER_NAME=${DOCKER_NAME}"
    echo "DOCKER_SQL_USER=${USERNAME}"
    echo "DOCKER_SQL_PASSWORD=${PASSWORD}"
    echo "DOCKER_ENVIRONMENT=Staging"
    echo "MINIO_ROOT_USER=${DOCKER_MINIO_USER}"
    echo "MINIO_ROOT_PASSWORD=${DOCKER_MINIO_PASSWORD}"
} >> .env

# Swap in all placeholders with sensitive or contextual data in the appsettings and make a tmp copy from it
sed -e "s/{catalog-server-name}/${DOCKER_NAME}-datasource/" \
    -e "s/{catalog-server-user-id}/$USERNAME/" \
    -e "s/{catalog-server-user-password}/$PASSWORD/" \
    -e "s/{docker_name-minio}/${DOCKER_NAME}-minio/" \
    ./src/UserAdmin/src/Smart.FA.Catalog.Web/appsettings.Staging.json > ./src/UserAdmin/src/Smart.FA.Catalog.Web/appsettings.Staging.tmp.json

# Replace the actual file with the tmp copy
mv ./src/UserAdmin/src/Smart.FA.Catalog.Web/appsettings.Staging.tmp.json ./src/UserAdmin/src/Smart.FA.Catalog.Web/appsettings.Staging.json

# If a SQL server container is not already running for cfa, then we create it
if [ "$( docker container inspect -f '{{.State.Running}}' "dev-cfa-datasource" )" != "true" ]; then

echo "dev-cfa-datasource not running"
docker stop dev-cfa-datasource || true
docker rm dev-cfa-datasource || true

docker build \
  -f "./docker/DB.Dockerfile" \
  -t "dev-cfa-datasource" \
  .
docker run -d \
  --name dev-cfa-datasource \
  -e "SA_PASSWORD=${PASSWORD}" \
  --network=cfa \
  dev-cfa-datasource
sleep 3
else
  echo "dev-cfa-datasource running"
fi

# If a Minio server container is not already running for cfa, then we create it
if [ "$( docker container inspect -f '{{.State.Running}}' "dev-cfa-minio" )" != "true" ]; then

echo "dev-cfa-minio not running"
docker stop dev-cfa-minio || true
docker rm dev-cfa-minio || true

docker run -d \
  --name dev-cfa-minio \
  -e "MINIO_ROOT_USER=${DOCKER_MINIO_USER}"  \
  -e "MINIO_ROOT_PASSWORD=${DOCKER_MINIO_PASSWORD}" \
  -v "/minio/data:/data" \
  -p "9001:9001" \
  -p "9000:9000" \
  --network=cfa \
  quay.io/minio/minio \
  server /data --console-address ":9001"
sleep 3
else
  echo "dev-cfa-minio running"
fi

# Use the docker compose file to run all other containers while passing environment variables
echo "build docker Web UserAdmin and Showcase app"
docker-compose --env-file ./.env build
echo "stop old docker"
docker-compose down || true
echo "run new docker."
docker-compose --env-file ./.env up -d
