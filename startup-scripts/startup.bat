echo "Deleting cfa network"
docker network rm local-cfa || true

echo "Stopping docker containers..."
docker-compose -f ./docker-compose-startup.yml stop

echo "Removing docker containers and images"
docker-compose -f ./docker-compose-startup.yml down --rmi all

echo "Re-creating docker images and running containers"
docker-compose -f ./docker-compose-startup.yml up --build --remove-orphans
exit
