echo "Stopping docker containers..."
docker-compose stop

echo "Removing docker containers and images"
docker-compose -f ./docker-compose-local.yml down --rmi all

echo "Re-creating docker images and running containers"
docker-compose -f ./docker-compose-local.yml up --remove-orphans
exit
