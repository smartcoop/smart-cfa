FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ARG Environment

ENV DEBIAN_FRONTEND noninteractive
ENV ASPNETCORE_ENVIRONMENT $Environment
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["../../src/Showcase/src/Smart.FA.Catalog.Showcase.Web/Smart.FA.Catalog.Showcase.Web.csproj", "src/Showcase/src/Smart.FA.Catalog.Showcase.Web/"]
COPY ["../../src/Showcase/src/Smart.FA.Catalog.Showcase.Localization/Smart.FA.Catalog.Showcase.Localization.csproj", "src/Showcase/src/Smart.FA.Catalog.Showcase.Localization/"]
COPY ["../../src/Showcase/src/Smart.FA.Catalog.Showcase.Domain/Smart.FA.Catalog.Showcase.Domain.csproj", "src/Showcase/src/Smart.FA.Catalog.Showcase.Domain/"]
COPY ["../../src/Showcase/src/Smart.FA.Catalog.Showcase.Infrastructure/Smart.FA.Catalog.Showcase.Infrastructure.csproj", "src/Showcase/src/Smart.FA.Catalog.Showcase.Infrastructure/"]

RUN dotnet restore "src/Showcase/src/Smart.FA.Catalog.Showcase.Web/Smart.FA.Catalog.Showcase.Web.csproj"
COPY . .
WORKDIR "/src/src/Showcase/src/Smart.FA.Catalog.Showcase.Web"
RUN dotnet build "Smart.FA.Catalog.Showcase.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Smart.FA.Catalog.Showcase.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install krb5 packages
RUN apt-get update && apt-get -y install cron
RUN apt remove krb5-config krb5-user
RUN apt install -y krb5-config
RUN apt install -y krb5-user
# create a cron job
COPY ./ktutil/cfa-cron /etc/cron.d/cfa-cron
COPY ./ktutil/refresh.sh /etc/cron.d/refresh.sh
# Give execution rights on the cron job
RUN chmod 0644 /etc/cron.d/cfa-cron
RUN chmod +x /etc/cron.d/refresh.sh
# Apply cron job
RUN crontab /etc/cron.d/cfa-cron

# ENTRYPOINT ["dotnet", "Smart.FA.Catalog.Web.dll"]
COPY ./launch_showcase.sh /app/launch_showcase.sh
RUN sed  -i "s/CipherString = DEFAULT@SECLEVEL=2/CipherString = DEFAULT@SECLEVEL=1/" /etc/ssl/openssl.cnf

RUN chmod +x /app/launch_showcase.sh
CMD /app/launch_showcase.sh
