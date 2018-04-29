$server = "jsonbenchmarksserver_server_1"
& docker stop $server
& docker rm $server
& docker-compose up --build --detach
& cd Benchmarks
& dotnet run -c Release
& docker stop $server
& docker rm $server
& cd ..