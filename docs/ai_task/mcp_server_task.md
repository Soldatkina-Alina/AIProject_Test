Анализ проекта
Архитектура проекта:

Это ASP.NET Core Web API приложение для управления промокодами
Используется многослойная архитектура с разделением на слои:
Core (доменные модели и абстракции)
DataAccess (репозитории и контекст БД)
WebHost (контроллеры и модели DTO)
Используется Entity Framework Core с SQLite
Проект настроен для работы в Docker
Ключевые сущности:

Customer (клиенты)
Employee (сотрудники)
Partner (партнеры)
Preference (предпочтения)
PromoCode (промокоды)
API методы:

CreateCustomerAsync - создание клиента
GetCustomerByIdAsync - получение клиента по ID
UpdateCustomerAsync - обновление клиента
DeleteCustomerAsync - удаление клиента
И другие методы для управления промокодами
План создания MCP сервера

## План реализации MCP сервера

### Шаг 1: Создание проекта MCP сервера

1. Создать папку `src/McpServer/`
2. Создать проект типа "MCP Server App"
3. Добавить в решение `PromoCodeFactory.sln`

### Шаг 2: Настройка зависимостей

1. Добавить зависимости для HTTP-клиента
2. Добавить зависимости для JSON сериализации
3. Настроить логирование

### Шаг 3: Создание HTTP клиента для PromoCodeFactory

1. Создать класс `PromoCodeFactoryApiClient`

2. Реализовать методы для всех необходимых API вызовов:

   - `CreateCustomerAsync`
   - `GetCustomerByIdAsync`
   - `UpdateCustomerAsync`
   - `DeleteCustomerAsync`

### Шаг 4: Создание MCP инструментов

1. Создать базовый класс для инструментов
2. Реализовать `CreateCustomerTool`
3. Реализовать `GetCustomerTool`
4. Реализовать `UpdateCustomerTool`
5. Реализовать `DeleteCustomerTool`

### Шаг 5: Настройка конфигурации

1. Создать `appsettings.json` с настройками подключения
2. Настроить DI контейнер
3. Добавить логирование

### Шаг 6: Docker интеграция

1. Создать `Dockerfile` для MCP сервера
2. Обновить `compose.yml` для запуска обоих сервисов
3. Настроить сетевое взаимодействие

### Шаг 7: Тестирование

1. Запустить оба сервиса
2. Протестировать MCP инструменты через Cline
3. Проверить логи и работу

## Структура файлов MCP сервера:

```javascript
src/McpServer/
├── McpServer.csproj
├── Program.cs
├── appsettings.json
├── Dockerfile
├── Services/
│   └── PromoCodeFactoryApiClient.cs
├── Tools/
│   ├── CreateCustomerTool.cs
│   ├── GetCustomerTool.cs
│   ├── UpdateCustomerTool.cs
│   └── DeleteCustomerTool.cs
└── Models/
    ├── CreateCustomerRequest.cs
    ├── CustomerResponse.cs
    └── ToolResult.cs
```

## Конкретные действия:

1. __Создание проекта:__

```bash
cd src
mkdir McpServer
cd McpServer
dotnet new mcp-server-app -n McpServer
```

2. __Добавление в решение:__

```bash
cd ..
dotnet sln PromoCodeFactory.sln add McpServer/McpServer.csproj
```

3. __Реализация HTTP клиента:__

- Создание класса для взаимодействия с PromoCodeFactory API
- Реализация всех необходимых методов

4. __Реализация инструментов:__

- Каждый инструмент будет иметь описание параметров
- Реализация валидации входных данных
- Обработка ошибок

5. __Настройка Docker:__

- Создание Dockerfile для MCP сервера
- Обновление compose.yml для запуска обоих сервисов


Преимущества такого подхода:
Простота интеграции с существующим API
Возможность вызова любых методов API из чата агента
Гибкость в добавлении новых инструментов
Сохранение существующей архитектуры