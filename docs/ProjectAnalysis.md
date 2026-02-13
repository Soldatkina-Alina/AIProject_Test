# Анализ проекта PromoCodeFactory

## Обзор

Проект **PromoCodeFactory** - это веб-приложение для управления промокодами, клиентами, партнерами и сотрудниками. Это REST API, построенное на архитектуре Clean Architecture.

## Архитектура

### Слоистая архитектура (Onion Architecture / Clean Architecture)
Проект разделен на три основные слоя:
- **Core** - доменная логика и модели
- **DataAccess** - доступ к данным и репозитории
- **WebHost** - API и контроллеры

```mermaid
graph LR
    A[WebHost<br/>(Controllers, Models)] --> B[DataAccess<br/>(Repositories, EF Core)]
    B --> C[Core<br/>(Domain Models, Abstractions)]
```

## Технологии и инструменты

### Backend
- **.NET 8.0** - основной фреймворк
- **ASP.NET Core MVC** - для построения REST API
- **Entity Framework Core** - ORM для работы с базой данных
- **SQLite** - используется в разработке как локальная база данных
- **NSwag** - для генерации OpenAPI/Swagger документации

### Инструменты CI/CD
- **GitLab CI** - pipeline для автоматического тестирования и развертывания
- **Docker** - контейнеризация приложения
- **Docker Compose** - управление многоконтейнерными приложениями

### Тестирование
- **xUnit** - тестовый фреймворк
- **Moq** (вероятно) - mocking framework

## Доменная модель

### Основные сущности

| Модель | Описание |
|--------|----------|
| **Employee** | Сотрудник с ролями и количеством примененных промокодов |
| **Role** | Роль сотрудника (например, менеджер, администратор) |
| **Customer** | Клиент с предпочтениями и полученными промокодами |
| **CustomerPreference** | Связь клиента и предпочтения (многие-ко-многим) |
| **Preference** | Предпочтения клиентов (например, категории товаров) |
| **PromoCode** | Промокод с номером, скидкой и сроком действия |
| **Partner** | Партнер, который предоставляет промокоды |
| **PartnerPromoCodeLimit** | Лимит на количество промокодов для партнера |

## Точка входа и запуск приложения

### Точка входа
- **Файл**: `src/PromoCodeFactory.WebHost/Program.cs`
- **Класс**: `Program`
- **Метод**: `Main()` - основной метод приложения

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}
```

### Конфигурация
- **Startup.cs** - конфигурация сервисов и pipeline
- **appsettings.json** - параметры конфигурации
- **appsettings.Development.json** - настройки для разработки

### База данных
- **SQLite** - используется по умолчанию (`PromoCodeFactoryDb.sqlite`)
- **Инициализация**: при запуске приложения автоматически создается и заполняется тестовыми данными

## API Контроллеры

### Доступные endpoints

| Контроллер | Путь | Описание |
|------------|------|----------|
| **CustomersController** | `/api/v1/customers` | Управление клиентами |
| **EmployeesController** | `/api/v1/employees` | Управление сотрудниками |
| **PartnersController** | `/api/v1/partners` | Управление партнерами и их лимитами |
| **PreferencesController** | `/api/v1/preferences` | Управление предпочтениями |
| **PromocodesController** | `/api/v1/promocodes` | Управление промокодами |
| **RolesController** | `/api/v1/roles` | Управление ролями сотрудников |

### Пример: PartnersController

```csharp
[ApiController]
[Route("api/v1/[controller]")]
public class PartnersController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<PartnerResponse>>> GetPartnersAsync()
    [HttpGet("{id}/limits/{limitId}")]
    public async Task<ActionResult<PartnerPromoCodeLimit>> GetPartnerLimitAsync(Guid id, Guid limitId)
    [HttpPost("{id}/limits")]
    public async Task<IActionResult> SetPartnerPromoCodeLimitAsync(Guid id, SetPartnerPromoCodeLimitRequest request)
    [HttpPost("{id}/canceledLimits")]
    public async Task<IActionResult> CancelPartnerPromoCodeLimitAsync(Guid id)
}
```

## Промежуточная стадия разработки

### Что уже реализовано
- Базовая архитектура и слоистая структура
- Доменные модели с реляциями
- Репозиторий pattern для доступа к данным
- API контроллеры с основными endpoints
- Автоматическая инициализация базы данных с тестовыми данными
- Swagger для документации API
- Юнит тесты для некоторых контроллеров

### Что нужно доделать

1. **Бизнес-логика**
   - Логика выдачи промокодов клиентам
   - Проверка лимитов партнёров при выдаче промокодов
   - Валидация данных на уровне домена

2. **Безопасность и авторизация**
   - Аутентификация пользователей
   - Авторизация по ролям
   - Защита API от атак (CORS, CSRF)

3. **Обработка ошибок**
   - Глобальная обработка исключений
   - Детализированные сообщения об ошибках
   - Логирование ошибок

4. **Тестирование**
   - Покрытие тестами всех контроллеров
   - Интеграционные тесты
   - Тесты на нагрузку

5. **Возможности API**
   - Фильтрация, сортировка и пагинация данных
   - Поиск по ключевым полям
   - Бэкап и восстановление данных

6. **Производственная конфигурация**
   - Поддержка других баз данных (PostgreSQL, SQL Server)
   - Кэширование данных
   - Настройки для масштабирования

## Запуск проекта

### Локальный запуск
```bash
cd src/PromoCodeFactory.WebHost
dotnet run
```

Приложение будет доступно по адресу `https://localhost:5001` (или `http://localhost:5000`). Swagger UI - `https://localhost:5001/swagger`.

### Запуск с Docker
```bash
docker compose up
```

## Заключение

Проект **PromoCodeFactory** имеет хорошую архитектуру и базовый набор функционала для управления промокодами. Но он находится в начальной стадии разработки и требует расширения функционала, улучшения безопасности и покрытия тестами. Архитектура Clean Architecture позволяет легко расширять приложение и добавлять новые функции без сильных изменений в существующий код.