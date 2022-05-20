FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ARG Environment

ENV DEBIAN_FRONTEND noninteractive
ENV ASPNETCORE_ENVIRONMENT $Environment
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["./src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Web/Smart.FA.Catalog.UserAdmin.Web.csproj", "src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Web/"]
COPY ["./src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Application/Smart.FA.Catalog.UserAdmin.Application.csproj", "src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Application/"]
COPY ["./src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Domain/Smart.FA.Catalog.UserAdmin.Domain.csproj", "src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Domain/"]
COPY ["./src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Infrastructure/Smart.FA.Catalog.UserAdmin.Infrastructure.csproj", "src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Infrastructure/"]

RUN dotnet restore "src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Web/Smart.FA.Catalog.UserAdmin.Web.csproj"
COPY . .
WORKDIR "/src/src/UserAdmin/src/Smart.FA.Catalog.UserAdmin.Web"
RUN dotnet build "Smart.FA.Catalog.UserAdmin.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Smart.FA.Catalog.UserAdmin.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install krb5 packages
RUN apt-get update && apt-get -y install cron
RUN apt remove krb5-config krb5-user
RUN apt install -y krb5-config
RUN apt install -y krb5-user
# create a cron job
COPY ./pipeline-scripts/ktutil/cfa-cron /etc/cron.d/cfa-cron
COPY ./pipeline-scripts/ktutil/refresh.sh /etc/cron.d/refresh.sh
# Give execution rights on the cron job
RUN chmod 0644 /etc/cron.d/cfa-cron
RUN chmod +x /etc/cron.d/refresh.sh
# Apply cron job
RUN crontab /etc/cron.d/cfa-cron

# ENTRYPOINT ["dotnet", "Smart.FA.Catalog.Web.dll"]
COPY ./pipeline-scripts/launch_useradmin.sh /app/launch_useradmin.sh
RUN sed  -i "s/CipherString = DEFAULT@SECLEVEL=2/CipherString = DEFAULT@SECLEVEL=1/" /etc/ssl/openssl.cnf

RUN chmod +x /app/launch_useradmin.sh
CMD /app/launch_useradmin.sh
