# sql server
FROM mcr.microsoft.com/mssql/server:2025-latest

EXPOSE 1433

USER root

# create backup folder
RUN mkdir -p /var/opt/mssql/backup

# entrypoint
COPY entrypoint.sh /

# files
WORKDIR /opt/
COPY db-init.sh .
COPY scripts .

RUN chmod +x db-init.sh
RUN chmod +x /entrypoint.sh

# user
USER mssql

CMD /bin/bash /appdev/entrypoint.sh