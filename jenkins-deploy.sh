#! /usr/bin/env bash

# This scripts is called by Jenkins whenever a branch is pushed to GitHub.

set -e

# Convert uppercase to lowercase.
DOCKER_NAME=$(echo cfa-${BRANCH_NAME} | sed -e 's/\(.*\)/\L\1/' -e 's/origin\/master/master/g' -e 's/origin\/feature\//feature-/g' -e 's/origin\/hotfix\//hotfix-/g' -e 's/feature\//feature-/g' -e 's/hotfix\//hotfix-/g')
DOCKER_HOSTNAME=$(echo ${DOCKER_NAME})

echo
if ! echo ${DOCKER_NAME} |  grep -q "^[a-z][a-z0-9_-]*$"; then
  echo "The branch does not use only alphanumeric characters and dashes. Exiting.";
  exit 1
fi

# TODO HTML-escape those strings, especially the subject.
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

echo "DOCKER_NAME=${DOCKER_NAME}" > .env
echo "DOCKER_SQL_USER=${USERNAME}" >> .env
echo "DOCKER_SQL_PASSWORD=${PASSWORD}" >> .env
echo "DOCKER_ENVIRONMENT=Staging" >> .env

sed -e "s/{catalog-server-name}/${DOCKER_NAME}-datasource/" \
    -e "s/{catalog-server-user-id}/$USERNAME/" \
    -e "s/{catalog-server-user-password}/$PASSWORD/" \
    ./src/Smart.FA.Catalog.Web/appsettings.Staging.json > ./src/Smart.FA.Catalog.Web/appsettings.Staging.tmp.json

mv ./src/Smart.FA.Catalog.Web/appsettings.Staging.tmp.json ./src/Smart.FA.Catalog.Web/appsettings.Staging.json


echo "build docker DB and API"
docker-compose --env-file ./.env build
echo "stop old docker"
docker-compose down || true
echo "run new docker."
docker-compose --env-file ./.env up -d
