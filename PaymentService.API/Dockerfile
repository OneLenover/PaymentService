# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ../PaymentService.DataAccess.Postgres/ ./PaymentService.DataAccess.Postgres/
COPY ./ ./PaymentService.API/

WORKDIR /src/PaymentService.API
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# Этап запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "PaymentService.API.dll"]
