{
    SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"
    PASSWORD=$(<$SCRIPT_DIR/src/cypress/util/dbpassword)

    dotnet ef database update \
        --project "../../Backend/Order.API/Order.API.csproj" \
        --connection "User ID=postgres;Password='$PASSWORD';Host=localhost;Port=5432;Database=Order.API;"

    dotnet run \
        --project "../../Backend/Order.API/Order.API.csproj" &
    api="$!"

    npm run start &
    npm="$!"
    read -s -n 1
} ||
{
    kill $api
    kill $npm
}
