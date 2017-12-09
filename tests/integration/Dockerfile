FROM microsoft/aspnetcore-build:2.0
WORKDIR /app
COPY . .
RUN dotnet restore
ENTRYPOINT ["dotnet", "test"]