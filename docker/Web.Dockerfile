FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Staging
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Web/Smart.FA.Catalog.UserAdmin.Web.csproj", "src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Web/"]
COPY ["./src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Application/Smart.FA.Catalog.UserAdmin.Application.csproj", "src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Application/"]
COPY ["./src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Domain/Smart.FA.Catalog.UserAdmin.Domain.csproj", "src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Domain/"]
COPY ["./src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Infrastructure/Smart.FA.Catalog.UserAdmin.Infrastructure.csproj", "src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Infrastructure/"]
COPY ["./src/Shared/Smart.FA.Catalog.Shared/Smart.FA.Catalog.Shared.csproj", "src/Shared/Smart.FA.Catalog.Shared/"]

RUN dotnet restore "src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Web/Smart.FA.Catalog.UserAdmin.Web.csproj"
COPY . .
WORKDIR "/src/src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Web"
RUN dotnet build "Smart.FA.Catalog.UserAdmin.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Smart.FA.Catalog.UserAdmin.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Smart.FA.Catalog.UserAdmin.Web.dll"]
