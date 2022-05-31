echo "Deleting cfa network"
docker network rm local-cfa || true

echo "Stopping docker containers..."
docker-compose stop

echo "Removing docker containers and images"
docker-compose -f ./docker-compose-local.yml down --rmi all

echo "Re-creating docker images and running containers"
docker-compose -f ./docker-compose-local.yml up  --force-recreate --build --remove-orphans
exit
