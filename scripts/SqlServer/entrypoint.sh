#!/bin/sh
#start SQL Server, start the script to create the DB and initial datas
echo 'starting database setup'
database=Stocqres
wait_time=30
password=Your_password123
host=127.0.0.1
echo 'Please wait while SQL Server 2017 warms up'

sleep "$wait_time"
echo 'Scripts starts running'

/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Your_password123" -i ./InitDb.sql
echo 'InitDb done'
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Your_password123" -d $database -i ./IdentityScript.sql
echo 'IdentityScript done'
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Your_password123" -d $database -i ./ProcessManager.sql
echo 'ProcessManager done'
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Your_password123" -d $database -i ./Aggregates.sql
echo 'Aggregates done'

echo 'Finished initializing the database'
