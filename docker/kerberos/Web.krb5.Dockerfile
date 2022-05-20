FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ARG Environment

ENV DEBIAN_FRONTEND noninteractive
ENV ASPNETCORE_ENVIRONMENT $Environment
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["../../src/UserAdmin/src/Smart.FA.Catalog.Web/Smart.FA.Catalog.Web.csproj", "src/UserAdmin/src/Smart.FA.Catalog.Web/"]
COPY ["../../src/UserAdmin/src/Smart.FA.Catalog.Application/Smart.FA.Catalog.Application.csproj", "src/UserAdmin/src/Smart.FA.Catalog.Application/"]
COPY ["../../src/UserAdmin/src/Smart.FA.Catalog.Core/Smart.FA.Catalog.Core.csproj", "src/UserAdmin/src/Smart.FA.Catalog.Core/"]
COPY ["../../src/UserAdmin/src/Smart.FA.Catalog.Infrastructure/Smart.FA.Catalog.Infrastructure.csproj", "src/UserAdmin/src/Smart.FA.Catalog.Infrastructure/"]

RUN dotnet restore "src/UserAdmin/src/Smart.FA.Catalog.Web/Smart.FA.Catalog.Web.csproj"
COPY . .
WORKDIR "/src/src/UserAdmin/src/Smart.FA.Catalog.Web"
RUN dotnet build "Smart.FA.Catalog.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Smart.FA.Catalog.Web.csproj" -c Release -o /app/publish

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
COPY ./launch_useradmin.sh /app/launch_useradmin.sh
RUN sed  -i "s/CipherString = DEFAULT@SECLEVEL=2/CipherString = DEFAULT@SECLEVEL=1/" /etc/ssl/openssl.cnf

RUN chmod +x /app/launch_useradmin.sh
CMD /app/launch_useradmin.sh
