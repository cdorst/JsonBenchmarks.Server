& docker-compose down --rmi all
& docker-compose up --build --detach
& cd Benchmarks
& dotnet run -c Release
& docker-compose down --rmi all
& cd ..