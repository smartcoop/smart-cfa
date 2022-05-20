FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Staging
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./src/Showcase/src/Smart.FA.Catalog.Showcase.Web/Smart.FA.Catalog.Showcase.Web.csproj", "src/Showcase/src/Smart.FA.Catalog.Showcase.Web/"]
COPY ["./src/Showcase/src/Smart.FA.Catalog.Showcase.Localization/Smart.FA.Catalog.Showcase.Localization.csproj", "src/Showcase/src/Smart.FA.Catalog.Showcase.Localization/"]
COPY ["./src/Showcase/src/Smart.FA.Catalog.Showcase.Domain/Smart.FA.Catalog.Showcase.Domain.csproj", "src/Showcase/src/Smart.FA.Catalog.Showcase.Domain/"]
COPY ["./src/Showcase/src/Smart.FA.Catalog.Showcase.Infrastructure/Smart.FA.Catalog.Showcase.Infrastructure.csproj", "src/Showcase/src/Smart.FA.Catalog.Showcase.Infrastructure/"]
COPY ["./src/Shared/Smart.FA.Catalog.Shared/Smart.FA.Catalog.Shared.csproj", "src/Shared/Smart.FA.Catalog.Shared/"]

RUN dotnet restore "src/Showcase/src/Smart.FA.Catalog.Showcase.Web/Smart.FA.Catalog.Showcase.Web.csproj"
COPY . .
WORKDIR "/src/src/Showcase/src/Smart.FA.Catalog.Showcase.Web"
RUN dotnet build "Smart.FA.Catalog.Showcase.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Smart.FA.Catalog.Showcase.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Smart.FA.Catalog.Showcase.Web.dll"]
