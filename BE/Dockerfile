FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
EXPOSE 80
WORKDIR /app
COPY . .
NTRYPOINT ["dotnet", "$(appName)"]