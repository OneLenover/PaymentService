# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем глобальные props, чтобы restore подтянул все пакеты
COPY Directory.Build.props .

# Копируем csproj всех проектов
COPY PaymentService/PaymentService.API/*.csproj PaymentService.API/
COPY PaymentService/PaymentService.DataAccess.Postgres/*.csproj PaymentService.DataAccess.Postgres/

# Восстанавливаем зависимости
RUN dotnet restore PaymentService.API/PaymentService.API.csproj

# Копируем весь код
COPY PaymentService/. .

WORKDIR /src/PaymentService.API
RUN dotnet publish -c Release -o /app

# Этап запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "PaymentService.API.dll"]