# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["CricketParkBooking.API.csproj", "./"]
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet build "CricketParkBooking.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "CricketParkBooking.API.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 80
ENTRYPOINT ["dotnet", "CricketParkBooking.API.dll"]
