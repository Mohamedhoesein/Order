try
{
    $password=Get-Content -Path $PSScriptRoot/src/cypress/util/dbpassword

    dotnet ef database update --project "../../Backend/Order.API/Order.API.csproj" --connection "User ID=postgres;Password='$password';Host=localhost;Port=5432;Database=Order.API;"

    $global:api = Start-Process -FilePath 'dotnet' -ArgumentList "run", "--project", "../../Backend/Order.API/Order.API.csproj" -PassThru

    while (!(Test-NetConnection localhost -Port 7058).TcpTestSucceeded) { Start-Sleep 1 }

    $global:npm = Start-Process "npm" -ArgumentList "run", "start" -PassThru

    while (!(Test-NetConnection localhost -Port 5001).TcpTestSucceeded) { Start-Sleep 1 }

    Read-Host "Press ENTER to continue"
}
finally
{
    Stop-Process -Id $global:api.ID
    Stop-Process -Id $global:npm.ID
    Stop-Process -Id (Get-NetTCPConnection -LocalPort 7058).OwningProcess
    Stop-Process -Id (Get-NetTCPConnection -LocalPort 5001).OwningProcess
}
