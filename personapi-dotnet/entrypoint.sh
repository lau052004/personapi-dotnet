#!/usr/bin/env bash
set -e
if [ ! -f /opt/mssql-tools/bin/sqlcmd ]; then
  apt-get update -qq
  ACCEPT_EULA=Y apt-get install -y -qq mssql-tools unixodbc-dev
fi
/opt/mssql/bin/sqlservr &
echo "Esperando a SQL Server..."
for i in {1..30}; do
  /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -Q "SELECT 1" && break
  sleep 2
done
echo "Ejecutando init.sql..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -i /usr/src/app/init.sql
wait
