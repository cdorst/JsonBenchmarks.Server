docker-compose up --build --detach
cd Benchmarks
dotnet run -c Release
cd ..
docker-compose down --rmi all