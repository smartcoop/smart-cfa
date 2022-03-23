FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Staging
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Smart.FA.Catalog.Web/Smart.FA.Catalog.Web.csproj", "src/Smart.FA.Catalog.Web/"]
COPY ["src/Smart.FA.Catalog.Application/Smart.FA.Catalog.Application.csproj", "src/Smart.FA.Catalog.Application/"]
COPY ["src/Smart.FA.Catalog.Core/Smart.FA.Catalog.Core.csproj", "src/Smart.FA.Catalog.Core/"]
COPY ["src/Smart.FA.Catalog.Infrastructure/Smart.FA.Catalog.Infrastructure.csproj", "src/Smart.FA.Catalog.Infrastructure/"]

RUN dotnet restore "src/Smart.FA.Catalog.Web/Smart.FA.Catalog.Web.csproj"
COPY . .
WORKDIR "/src/src/Smart.FA.Catalog.Web"
RUN dotnet build "Smart.FA.Catalog.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Smart.FA.Catalog.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Smart.FA.Catalog.Web.dll"]
