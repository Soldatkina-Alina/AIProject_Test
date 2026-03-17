# Инструкция по запуску и проверке контейнеров

## Предварительные требования
- Установлен Docker Desktop
- Установлен Docker Compose

## Запуск контейнеров
1. Откройте командную строку или PowerShell
2. Перейдите в директорию с проектом: `cd src`
3. Запустите контейнеры: `docker-compose up -d --build`

## Проверка работы контейнеров
После запуска проверьте статус контейнеров: `docker-compose ps`

Должно быть две записи:
- src-promocodefactory-1 (PromoCodeFactory.WebHost)
- src-mcpserver-1 (McpServer)

## Проверка доступности API

### 1. PromoCodeFactory.WebHost API
- **Базовый URL**: http://localhost:5001/api/v1
- **Получение всех клиентов**: GET http://localhost:5001/api/v1/customers
- **Пример запроса в PowerShell**:
  ```powershell
  Invoke-RestMethod -Uri 'http://localhost:5001/api/v1/customers' -Method GET
  ```

### 2. McpServer API
- **Базовый URL**: http://localhost:5002/api/mcp
- **Получение всех клиентов**: GET http://localhost:5002/api/mcp/get-all-customers
- **Получение клиента по ID**: GET http://localhost:5002/api/mcp/get-customer/{customerId}
- **Пример запроса в PowerShell**:
  ```powershell
  Invoke-RestMethod -Uri 'http://localhost:5002/api/mcp/get-all-customers' -Method GET
  ```

## Доступ к логиам
- **PromoCodeFactory.WebHost**: `docker-compose logs promocodefactory`
- **McpServer**: `docker-compose logs mcpserver`

## Остановка контейнеров
- Остановить и удалить контейнеры: `docker-compose down`
- Остановить контейнеры без удаления: `docker-compose stop`

## Локальное тестирование
Если нужно запустить приложения локально (не в Docker):
1. Запустите PromoCodeFactory.WebHost: `cd src/PromoCodeFactory.WebHost; dotnet run`
2. Запустите McpServer: `cd src/McpServer; dotnet run`

## Дополнительная информация
- Контейнеры используют порты: 5001 (PromoCodeFactory), 5002 (McpServer)
- База данных SQLite сохраняется в директории `src/data`
- Логи сервера сохраняются в контейнерах и доступны через `docker-compose logs`