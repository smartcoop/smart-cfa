#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Staging
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UserAdmin/src/Smart.FA.Catalog.AccountSimulator/Smart.FA.Catalog.AccountSimulator.csproj", "src/UserAdmin/src/Smart.FA.Catalog.AccountSimulator/"]

RUN dotnet restore "src/UserAdmin/src/Smart.FA.Catalog.AccountSimulator/Smart.FA.Catalog.AccountSimulator.csproj"
COPY . .
WORKDIR "/src/src/UserAdmin/src/Smart.FA.Catalog.AccountSimulator"
RUN dotnet build "Smart.FA.Catalog.AccountSimulator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Smart.FA.Catalog.AccountSimulator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Smart.FA.Catalog.AccountSimulator.dll"]
