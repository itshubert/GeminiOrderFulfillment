# Use the official .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copy solution file and project files
COPY GeminiOrderFulfillment.sln ./
COPY src/GeminiOrderFulfillment.Api/GeminiOrderFulfillment.Api.csproj ./src/GeminiOrderFulfillment.Api/
COPY src/GeminiOrderFulfillment.Application/GeminiOrderFulfillment.Application.csproj ./src/GeminiOrderFulfillment.Application/
COPY src/GeminiOrderFulfillment.Contracts/GeminiOrderFulfillment.Contracts.csproj ./src/GeminiOrderFulfillment.Contracts/
COPY src/GeminiOrderFulfillment.Domain/GeminiOrderFulfillment.Domain.csproj ./src/GeminiOrderFulfillment.Domain/
COPY src/GeminiOrderFulfillment.Infrastructure/GeminiOrderFulfillment.Infrastructure.csproj ./src/GeminiOrderFulfillment.Infrastructure/

# Restore NuGet packages
RUN dotnet restore GeminiOrderFulfillment.sln

# Copy the entire source code
COPY . ./

# Build and publish the API project
RUN dotnet publish src/GeminiOrderFulfillment.Api/GeminiOrderFulfillment.Api.csproj -c Release -o out

# Use the official .NET 9.0 ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build-env /app/out .

# Create a non-root user for security
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser


# Set the entry point for the container
ENTRYPOINT ["dotnet", "GeminiOrderFulfillment.Api.dll"]