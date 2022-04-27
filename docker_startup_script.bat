:: Stop potentially running services
echo "Stopping docker containers..."
docker-compose stop

:: Down and remove images
echo "Removing docker containers and images"
docker-compose -f ./docker-compose-local.yml down --rmi all

:: launch specific docker compose (local) and remove orphans (previous containers that could have been removed from a docker compose)
echo "Re-creating docker images and running containers"
docker-compose -f ./docker-compose-local.yml up --remove-orphans

exit
