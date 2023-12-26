{
    PASSWORD=$(<dbpassword)

    dotnet ef database update \
        --project "../../Backend/Order.API/Order.API.csproj" \
        --connection "User ID=postgres;Password='$PASSWORD';Host=localhost;Port=5432;Database=Order.API;"

    dotnet run \
        --project "../../Backend/Order.API/Order.API.csproj" &
    api="$!"

    npm ng serve &
    npm="$!"
    read -s -n 1
    killall Order.API
    kill -9 $api
    kill -9 $npm
} ||
{
    killall Order.API
    kill -9 $api
    kill -9 $npm
}
