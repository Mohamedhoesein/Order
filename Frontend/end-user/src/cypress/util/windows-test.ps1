try
{
    $password=Get-Content -Path $PSScriptRoot/dbpassword

    dotnet ef database update `
        --project "../../Backend/Order.API/Order.API.csproj" `
        --connection "User ID=postgres;Password='$password';Host=localhost;Port=5432;Database=Order.Test.EndUser;"

    $global:api = Start-Process `
        -FilePath "dotnet" `
        -ArgumentList "run", `
            "--project", "../../Backend/Order.API/Order.API.csproj", `
            "--ConnectionStrings:OrderContextConnection=`"User ID=postgres;Password='$password';Host=localhost;Port=5432;Database=Order.Test.EndUser;`"" `
        -PassThru

    while (!(Test-NetConnection localhost -Port 7058).TcpTestSucceeded) { Start-Sleep 1 }

    $global:npm = Start-Process "npm" -ArgumentList "run", "start" -PassThru

    while (!(Test-NetConnection localhost -Port 5001).TcpTestSucceeded) { Start-Sleep 1 }

    npm run cypress:open
}
finally
{
    Stop-Process -Id $global:api.ID
    Stop-Process -Id $global:npm.ID
    Stop-Process -Id (Get-NetTCPConnection -LocalPort 7058).OwningProcess
    Stop-Process -Id (Get-NetTCPConnection -LocalPort 5001).OwningProcess

    $env:PGPASSWORD=Get-Content -Path $PSScriptRoot/dbpassword; psql -U postgres --command="DROP DATABASE `"`"Order.Test.EndUser`"`";"
}
