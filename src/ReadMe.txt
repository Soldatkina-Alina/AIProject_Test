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

Cline:
    "PromoCodeFactoryMcp": {
      "autoApprove": [
        "CreateCustomer",
        "GetCustomerById",
        "GetAllCustomers",
        "UpdateCustomer",
        "DeleteCustomer",
        "update_customer",
        "delete_customer",
        "get_customer_by_id",
        "create_customer",
        "get_all_customers"
      ],
      "disabled": false,
      "timeout": 30000,
      "type": "stdio",
      "command": "docker",
      "args": [
        "exec",
        "-i",
        "src-mcpserver-1",
        "dotnet",
        "McpServer.dll",
        "--mcp"
      ]
    }
  }

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
- **Создание клиента**: POST http://localhost:5002/api/mcp/create-customer
- **Обновление клиента**: PUT http://localhost:5002/api/mcp/update-customer/{customerId}
- **Удаление клиента**: DELETE http://localhost:5002/api/mcp/delete-customer/{customerId}

#### Примеры запросов в PowerShell:
```powershell
# Получение всех клиентов
Invoke-RestMethod -Uri 'http://localhost:5002/api/mcp/get-all-customers' -Method GET

# Получение клиента по ID
Invoke-RestMethod -Uri 'http://localhost:5002/api/mcp/get-customer/a6c8c6b1-4349-45b0-ab31-244740aaf0f0' -Method GET

# Создание клиента
$body = @{
    FirstName = "Петр"
    LastName = "Иванов"
    Email = "petr.ivanov@example.com"
} | ConvertTo-Json
Invoke-RestMethod -Uri 'http://localhost:5002/api/mcp/create-customer' -Method POST -ContentType 'application/json' -Body $body

# Обновление клиента
$body = @{
    FirstName = "Петр"
    LastName = "Иванов"
    Email = "petr.ivanov.updated@example.com"
} | ConvertTo-Json
Invoke-RestMethod -Uri 'http://localhost:5002/api/mcp/update-customer/a6c8c6b1-4349-45b0-ab31-244740aaf0f0' -Method PUT -ContentType 'application/json' -Body $body

# Удаление клиента
Invoke-RestMethod -Uri 'http://localhost:5002/api/mcp/delete-customer/a6c8c6b1-4349-45b0-ab31-244740aaf0f0' -Method DELETE
```

## Использование MCP сервера через Cline
Cline автоматически подключается к McpServer при запуске. Для использования инструментов через Cline:

1. Убедитесь, что контейнеры запущены (docker-compose up -d)
2. Откройте Cline и используйте инструменты напрямую
3. Доступные инструменты:
   - CreateCustomer
   - GetCustomerById
   - GetAllCustomers
   - UpdateCustomer
   - DeleteCustomer

Пример использования через Cline:
```
[M] Выбери инструмент "GetAllCustomers"
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