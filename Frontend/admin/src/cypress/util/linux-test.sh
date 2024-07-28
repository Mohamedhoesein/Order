{
    PASSWORD=$(<dbpassword)

    dotnet ef database update \
        --project "../../Backend/Order.API/Order.API.csproj" \
        --connection "User ID=postgres;Password='$PASSWORD';Host=localhost;Port=5432;Database=Order.Test.Admin;"

    dotnet run \
        --project "../../Backend/Order.API/Order.API.csproj" \
        --ConnectionStrings:OrderContextConnection="User ID=postgres;Password='$PASSWORD';Host=localhost;Port=5432;Database=Order.Test.Admin;" \
        --EmailConfiguration:DropEmailDirectory="$(pwd)/src/cypress/email" &
    api="$!"

    npm run dev &
    npm="$!"

    npm run cypress:open

    killall Order.API
    killall node
    kill -9 $api
    kill -9 $npm
    PGPASSWORD=$PASSWORD psql -U postgres --command="DROP DATABASE \"Order.Test.Admin\";"
} ||
{
    killall Order.API
    killall node
    kill -9 $api
    kill -9 $npm

    PGPASSWORD=$(<dbpassword) psql -U postgres --command="DROP DATABASE \"Order.Test.Admin\";"
}
