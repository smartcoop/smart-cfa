# Catalogue Formateurs Associés

## Structure of the solution:

The solution is divided in two applications:

- A administrative web interface for trainers to add, update and delete their trainings and update their profile called [UserAdmin](./src/UserAdmin/src/Smart.FA.Catalog.Web)
- A public website showcasing the different trainings offered by trainers called [Showcase](./src/Showcase/src/Smart.FA.Catalog.Showcase.Web)

## UserAdmin
### Building

The project can be built in local either with a local database like SQLExpress or in docker containers using the docker engine.

#### Local Development
The solution comes with default local development settings.
However one can override them by creating a `appsettings.Local.json`. If the file exists it will have precedence
over every appsettings*.json files).
Default connection string is
> Server=(LocalDB)\MSSQLLocalDB; Database=Catalog; Integrated Security=true;

> appsettings.Local.json is part of the gitignore

A S3 Storage like minio is necessary on port 9000 (by default) in order for the web app to work (it uses it to store files).
Launching the `startup.bat` file will create and run a minio docker image with the necessary setup (bucket, user creation, and volume mapping on host drive).

The application can also be debugged exclusively inside docker containers.
Launching the `docker_startup_script.bat` file will create and run the docker images with the proper setup.

#### Docker compose
Once docker engine is installed on your system, you can simply use the `docker-compose-local.yml` to launch required containers
to run the application. By Default, the web client should start on port **5000**
> command: docker-compose -f *{root of the project}*/docker-compose-local.yml up

It should run the web application, a S3 storage server and a sql server on the same docker network.

### Building and running the tests

#### Using the terminal

Simply run *dotnet test* with the name of the solution.

#### Using Jetbrain rider

*Tests > Run all Tests from Solution*

 > Note that the integrations tests are relying on an external database to execute the test.
It will create, execute and drop the database by test batches.
The connection is specified in the `ConnectionSetup` class.
Troubleshooting:

### Deployment

Development environment are created dynamically in docker containers and are accessible on Smart's local network.
The url are generated based on the name of the branch or the pr number with the template *{branch-name}.cfa.smartcoop.dev*.

*e.g: if PR 24 has been created, waiting for approval, and it can compiled, the web app will be available on url `https://pr-24.cfa.smartcoop.dev/`*

---
## Showcase
