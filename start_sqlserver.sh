#!/bin/bash

# username: sa
# password: Password_123
docker run --rm --name sqlserver -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Password_123" -p 1433:1433 -p 1434:1434 mcr.microsoft.com/mssql/server:2019-latest
