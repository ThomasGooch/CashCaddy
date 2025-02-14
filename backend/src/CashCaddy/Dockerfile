# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the project files and restore any dependencies
COPY ./*.csproj ./
RUN dotnet restore

# Copy the remaining source code and build the application
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official ASP.NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /app/out .

# Copy the certificate file
COPY mckesson.root.crt /usr/local/share/ca-certificates/mckesson.root.crt

# Update the CA certificates
RUN update-ca-certificates

# Expose the port the application runs on
EXPOSE 5000

# Run the application
ENTRYPOINT ["dotnet", "CashCaddy.API.dll"]