# Catalogue Formateurs Associés

## Structure of the solution:

The solution is divided in two applications:

- A administrative web interface for trainers to add, update and delete their trainings and update their profile called [UserAdmin](./src/UserAdmin/src/Smart.FA.Catalog.Web)
- A public website showcasing the different trainings offered by trainers called [Showcase](./src/Showcase/src/Smart.FA.Catalog.Showcase.Web)


### Building

The project can be built in local either with a local database like SQLExpress or in docker containers using the docker engine.

#### Step-by-step to launch CFA in "full dockerized mode"

- Rename [template.appsettings.Local.json](./src/UserAdmin/src/Smart.FA.Catalog.Web/template.appsettings.Local.json) into `appsettings.Local.json` located in project src/UserAdmin/src/SMart.FA.Catalog.Web
- Make sure Docker is running on the computer (in Windows, simply launch Docker Desktop, in Linux/Macos make sure a docker daemon is running).
- Launch a terminal in the root directory of the cfa repository (in the folder smart-cfa)
- Launch the script [docker_startup_script.bat](./startup-scripts/docker_startup_script.bat) in the terminal
- After a while (it can take some time when launched for the first time), you should see a running minio docker container bond on port 9000, a running useradmin docker contrainer, a running datasource docker container and a stopped minio-client container.
  The *useradmin container* is the administrator space for _"Catalogue Formateurs Associés"_.
- The *showcase container* is the public space for _"Catalogue Formateurs Associés"_.
  The *minio server container* is the file server used for storing the files of _"Catalogue Formateurs Associés"_.
  The *minio client container* connects to minio server and prepares the seed data for the minio server: it creates a bucket, a user for common operation and assign him a group and read/write policy.
  The *datasource* container is a containerized sql server instance.

#### Step-by-step to launch CFA in "minimal dockerized mode"

When running cfa a local instance of minio - the file server used by cfa - needs to run with basic settings.
A script is already available in the solution to create a minio docker container with the basic set-up.

- Make sure Docker is running on the computer (in Windows, simply launch Docker Desktop, in Linux/Macos make sure a docker daemon is running).
- Launch the script [startup.bat](./startup-scripts/startup.bat) in the terminal
- After a while (it can take some time when launched for the first time), you should see a running minio docker container bond on port 9000.
- Launch Smart.FA.Catalog.Web project (dotnet run ./src/UserAdmin/src/Smart.FA.Catalog.Web/Smart.FA.Catalog.Web.csproj)
- Launch Smart.FA.Catalog.Showcase.Web project (dotnet run ./src/Showcase/src/Smart.FA.Catalog.Showcase.Web/Smart.FA.Catalog.Showcase.Web.csproj)


Note: The solution comes with default local development settings (`appsettings.Development.json`) in both showcase and useradmin web projects.
However one can override them by creating a `appsettings.Local.json`. If the file exists it will have precedence
over every other appsettings*.json files).
A templated `appsettings.Local.json` has been created under the name [template.appsettings.Local.json](./src/UserAdmin/src/Smart.FA.Catalog.Web/template.appsettings.Local.json). Rename it to `appsettings.Local.json` if you want to use it as the current settings for the app.
Default connection string is
> Server=(LocalDB)\MSSQLLocalDB; Database=Catalog; Integrated Security=true; (Default SQL Server instance when SQLExpress is installed)

> appsettings.Local.json is part of the gitignore and will not be versioned

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
The url are generated based on the name of the branch or the pr number with the template *{branch-name}.cfa.smartcoop.dev* for useradmin and *{branch-name}-showcase.cfa.smartcoop.dev*.

*e.g: if PR 24 has been created, waiting for approval, and it can compiled, the web app will be available on url `https://pr-24.cfa.smartcoop.dev/` for useradmin and `https://pr-24-showcase.cfa.smartcoop.dev/` for showcase*

---
