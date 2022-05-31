FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Staging
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./src/UserAdmin/src/Smart.FA.Catalog.Web/Smart.FA.Catalog.Web.csproj", "src/UserAdmin/src/Smart.FA.Catalog.Web/"]
COPY ["./src/UserAdmin/src/Smart.FA.Catalog.Application/Smart.FA.Catalog.Application.csproj", "src/UserAdmin/src/Smart.FA.Catalog.Application/"]
COPY ["./src/UserAdmin/src/Smart.FA.Catalog.Core/Smart.FA.Catalog.Core.csproj", "src/UserAdmin/src/Smart.FA.Catalog.Core/"]
COPY ["./src/UserAdmin/src/Smart.FA.Catalog.Infrastructure/Smart.FA.Catalog.Infrastructure.csproj", "src/UserAdmin/src/Smart.FA.Catalog.Infrastructure/"]
COPY ["./src/Shared/Smart.FA.Catalog.Shared/Smart.FA.Catalog.Shared.csproj", "src/Shared/Smart.FA.Catalog.Shared/"]

RUN dotnet restore "src/UserAdmin/src/Smart.FA.Catalog.Web/Smart.FA.Catalog.Web.csproj"
COPY . .
WORKDIR "/src/src/UserAdmin/src/Smart.FA.Catalog.Web"
RUN dotnet build "Smart.FA.Catalog.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Smart.FA.Catalog.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Smart.FA.Catalog.Web.dll"]
