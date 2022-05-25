----
title : Showcase
----

# Overview / Technical stack

The solution is written in C# using the .NET 6.0 and uses EntityFramework 6.0.4.

It is deployed through Jenkins on Microsoft IIS servers and reverse-proxied by
Nginx.

It is a razor page project.


# Backing services (dependencies)

* Smart.FA.Catalog.Shared
* Smart.FA.Catalog.Showcase.Domain
* Smart.FA.Catalog.Showcase.Infrastructure
* Smart.FA.Catalog.Showcase.Localization

Showcase uses SMart database running on Microsoft SQL Server. It can be accessed
via Microsoft SQL Server Management Studio with Active Directory Windows
authentication (sometimes also called Integrated security or Trusted connection).

| Environment | SQL Server          |
| ----------- | ------------------- |
| production  | `SERVSQL3`          |
| stage       | `SRVWSTGSQL\SQLSTG` |

Showcase uses sql views  :

* [Cfa].[v_TrainerDetails]
* [Cfa].[v_TrainerList]
* [Cfa].[v_TrainingDetails]
* [Cfa].[v_TrainingList]

All scripts can be found in Smart.FA.Catalog.Showcase.Insfrustruct.Data.Scripts


# Consuming services (reverse dependencies)

# Deployment

## Running the application

Showcase is manually deployed through [Jenkins](https://jenkins.smartbe.be).


| Environment  | URL                                                                                                               |
| -----------  | ----------------------------------------------------------------------------------------------------------------- |         
| stage        | [COOP-STAGE/job/smart-cfa.STAGE] (https://jenkins.smartbe.be/view/stage/job/COOP-STAGE/job/smart-cfa.STAGE/)      |
| prod         | [COOP-PROD/job/smart-cfa.PROD] (https://jenkins.smartbe.be/view/production/job/COOP-PROD/job/smart-cfa.PROD/)     |


## Checking the application is healthy


# Development

In the Showcase LaunchSettings.json file, edit the environment to use (this will affect target databases and
services. The posible values are `Development`, `PreProduction`, `Production`:
``` json
    "profiles": {
    "Smart.FA.Catalog.Showcase.Web": {
     [...]
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
     [...]
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
```
Debugging is possible through breakpoints with your preferred IDE (Visual
Studio, Rider, ...).

Each environment is accessible on their domains:

| Environment       | Web service domain                                                                        |
| ----------------- | ----------------------------------------------------------------------------------------- |
| production        | [learning.smart.coop](https://learning.smart.coop/)                                       |
| stage             | [learning-stage.smartcoop.dev](https://learning-stage.smartcoop.dev/)                     |
| local             | [localhost:7019](https://localhost:7019); [localhost:5019](http://localhost:5019)         |

#Users and rights

Application has no special rights.
