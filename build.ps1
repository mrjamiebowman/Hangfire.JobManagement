Clear-Host

dos2unix .\.docker\mssql\db-init.sh
dos2unix .\.docker\mssql\entrypoint.sh

docker-compose build --no-cache