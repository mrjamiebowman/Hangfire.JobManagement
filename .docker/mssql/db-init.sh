#!/bin/bash

# wait for the SQL Server to come up
sleep 45s

echo "[+] Running SQL Setup Script"

# run the setup script to create the DB and the schema in the DB
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 5yyEj47ZlXITk39o9#r -d master -i /appdev/sql/db.sql

exit 0