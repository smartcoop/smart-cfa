#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Development
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Smart.FA.Catalog.Web/Web.csproj", "src/Smart.FA.Catalog.Web/"]
COPY ["src/Smart.FA.Catalog.Application/Application.csproj", "src/Smart.FA.Catalog.Application/"]
COPY ["src/Smart.FA.Catalog.Core/Core.csproj", "src/Smart.FA.Catalog.Core/"]
COPY ["src/Smart.FA.Catalog.Infrastructure/Infrastructure.csproj", "src/Smart.FA.Catalog.Infrastructure/"]

RUN dotnet restore "src/Smart.FA.Catalog.Web/Web.csproj"
COPY . .
WORKDIR "/src/src/Smart.FA.Catalog.Web"
RUN dotnet build "Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]
