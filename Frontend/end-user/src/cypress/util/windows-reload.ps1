$env:PGPASSWORD=Get-Content -Path $PSScriptRoot/dbpassword; psql -U postgres -d Order.Test.EndUser --file=$PSScriptRoot/reload.sql
Remove-Item -path $PSScriptRoot/../email -Recurse
New-Item -ItemType Directory -Path $PSScriptRoot/../email