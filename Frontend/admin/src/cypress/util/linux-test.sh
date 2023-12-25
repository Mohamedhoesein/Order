{
    PASSWORD=$(<dbpassword)

    dotnet ef database update \
        --project "../../Backend/Order.API/Order.API.csproj" \
        --connection "User ID=postgres;Password='$PASSWORD';Host=localhost;Port=5432;Database=Order.Test.EndUser;"

    dotnet run \
        --project "../../Backend/Order.API/Order.API.csproj" \
        --ConnectionStrings:OrderContextConnection="User ID=postgres;Password='$PASSWORD';Host=localhost;Port=5432;Database=Order.Test.EndUser;" \
        --EmailConfiguration:DropEmailDirectory="$(pwd)/src/cypress/email" &
    api="$!"

    ng serve &
    npm="$!"

    npm run cypress:open

    killall Order.API
    kill -9 $api
    kill -9 $npm
    dotnet ef database update 0 \
        --project "../../Backend/Order.API/Order.API.csproj" \
        --connection "User ID=postgres;Password='$PASSWORD';Host=localhost;Port=5432;Database=Order.Test.EndUser;"
} ||
{
    PASSWORD=$(<dbpassword)

    killall Order.API
    kill -9 $api
    kill -9 $npm

    dotnet ef database update 0 \
        --project "../../Backend/Order.API/Order.API.csproj" \
        --connection "User ID=postgres;Password='$PASSWORD';Host=localhost;Port=5432;Database=Order.Test.EndUser;"
}
