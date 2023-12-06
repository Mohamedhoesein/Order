SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"

TEMP=$(<$SCRIPT_DIR/dbpassword)
PGPASSWORD=$TEMP psql -U postgres -d Order.Test.Admin --file=$SCRIPT_DIR/reload.sql
rm -rf $SCRIPT_DIR/../email
mkdir $SCRIPT_DIR/../email