echo "Stopping docker containers..."
docker-compose -f ../docker/docker-compose-startup.yml stop

echo "Removing docker containers and images"
docker-compose -f ../docker/docker-compose-startup.yml down --rmi all

echo "Re-creating docker images and running containers"
docker-compose -f ../docker/docker-compose-startup.yml up
exit
