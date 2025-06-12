# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy all files and build
COPY . ./
RUN dotnet publish -c Release -o out

# Use the runtime image for smaller size
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Expose port 8080 for the web API
EXPOSE 8080

# Copy the published app from build stage
COPY --from=build /app/out ./

# Start the app
ENTRYPOINT ["dotnet", "SimpleApi.dll"]
