dotnet publish src/MyWebApiApp.csproj -c Release -r linux-x64 -o ../publish &&^
docker-compose -f docker-compose.yml build &&^
docker-compose -f docker-compose.yml up -d --force-recreate &&^
dotnet test tests/integration/MyWebApiApp.IntegrationTests.csproj
docker-compose -f docker-compose.yml down