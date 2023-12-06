{
    SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"
    PASSWORD=$(<$SCRIPT_DIR/dbpassword)

    dotnet ef database update \
        --project "../../Backend/Order.API/Order.API.csproj" \
        --connection "User ID=postgres;Password='$PASSWORD';Host=localhost;Port=5432;Database=Order.Test.Admin;"

    dotnet run \
        --project "../../Backend/Order.API/Order.API.csproj" \
        --ConnectionStrings:OrderContextConnection="User ID=postgres;Password='$PASSWORD';Host=localhost;Port=5432;Database=Order.Test.Admin;" &
    api="$!"

    npm run start &
    npm="$!"

    npm run cypress:open
} ||
{
    kill $api
    kill $npm

    TEMP=$(<$SCRIPT_DIR/dbpassword)
    PGPASSWORD=$TEMP psql -U postgres --command="DROP DATABASE \"Order.Test.Admin\";"
}
