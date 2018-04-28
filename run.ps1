& docker stop jsonbenchmarksserver_server_1
& docker rm jsonbenchmarksserver_server_1
& docker-compose up --build --detach
& cd Benchmarks
& dotnet run -c Release
& docker stop jsonbenchmarksserver_server_1
& docker rm jsonbenchmarksserver_server_1
& cd ..