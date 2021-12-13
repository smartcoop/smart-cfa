FROM mcr.microsoft.com/mssql/server:2019-latest

USER root

EXPOSE 1433


ENV SA_PASSWORD localStoragePassword!
ENV ACCEPT_EULA Y

RUN /opt/mssql/bin/sqlservr --accept-eula & sleep 70 \
   && /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'localStoragePassword!' \
   && pkill sqlservr 

# ENTRYPOINT ["/opt/mssql-tools/bin/sqlcmd", "-S localhost -U SA - P localStoragePassword!"]
