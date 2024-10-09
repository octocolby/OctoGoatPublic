#!/bin/sh
ls -latr

# Start crond in background
#crond
sleep 10 && curl -v -X POST -H \"CRON: \$HOSTNAME\" 'localhost:8080/populate?aaa=89w37f4yj5c9q8w3y4560n9f78c23y4cf6098234yd6o9x87y3456' &

# Start Beholder
dotnet OctoGoat.dll
