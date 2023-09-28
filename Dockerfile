FROM mcr.microsoft.com/mssql/server:2019-latest
EXPOSE 80
# Expose port 1433 in case accesing from other container
EXPOSE 1433

WORKDIR /usr/src/app
COPY . /usr/src/app

# Set environmment variables, not to have to write themm with docker run command
ENV SA_PASSWORD Strong_Password!
ENV ACCEPT_EULA Y

# Run Microsoft SQl Server and initialization script (at the same time)
RUN (/opt/mssql/bin/sqlservr &) && ( echo "SQLServer started" && sleep 30s ) \
    # Run the setup script to create the DB and the schema in the DB
    # Note: make sure that your password matches what is in the Dockerfile
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Strong_Password! -i create-database.sql