SERVER="jsonbenchmarksserver_server_1"
# compute current directory
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
ROOT=$DIR/..

docker stop $SERVER
docker rm $SERVER
docker-compose up --build --detach
cd $ROOT/Benchmarks
dotnet run -c Release
docker stop $SERVER
docker rm $SERVER
cd $ROOT