:: Using a little trick (goto instruction will be interpreted as a command which goes to label '(){' in windows but as a function in linux)
:: we can make a script compatible for both linux and windows os, we just need to give execution rights to the file
:: https://nastytester.com/posts/script-that-works-in-windows-and-linux.html
goto(){
    # Stop potentially running services
    echo "Stopping docker containers..."
    docker-compose stop

    # Down and remove images
    echo "Removing docker containers and images"
    docker-compose -f ./docker-compose-local.yml down --rmi all

    # launch specific docker compose (local) and remove orphans (previous containers that could have been removed from a docker compose)
    echo "Re-creating docker images and running containers"
    docker-compose -f ./docker-compose-local.yml up --remove-orphans
}

goto $@
exit

:(){
:: Stop potentially running services
echo "Stopping docker containers..."
docker compose stop

:: Down and remove images
echo "Removing docker containers and images"
docker compose -f ./docker-compose-local.yml down --rmi all

:: launch specific docker compose (local) and remove orphans (previous containers that could have been removed from a docker compose)
echo "Re-creating docker images and running containers"
docker compose -f ./docker-compose-local.yml up --remove-orphans

exit
