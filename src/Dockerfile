# Stage 1: build the source in a container by using the microsoft/aspnetcore-build image (it comes with the .NET CORE SDK)
FROM microsoft/aspnetcore-build AS builder
WORKDIR /source

# caches restore result by copying csproj file separately
COPY MyWebApiApp.csproj .
RUN dotnet restore

# copies the rest of your code
COPY . .
RUN dotnet publish --output /app/ --configuration Release

# Stage 2: move and run the build output to an environment which is more similar to production (just the .NET Core runtime installed)
FROM microsoft/aspnetcore
WORKDIR /app
COPY --from=builder /app .
ENV ASPNETCORE_ENVIRONMENT Production
ENTRYPOINT ["dotnet", "MyWebApiApp.dll"]