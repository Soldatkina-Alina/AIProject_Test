# Подзадачи для проекта PromoCodeFactory

## Задача 1: Реализовать логику выдачи промокодов клиентам

| № | Подзадача | Описание | Файлы |
|---|-----------|----------|-------|
| 1.1 | Создать модель запроса для выдачи промокода | Добавить класс `GivePromoCodeRequest` с полями CustomerId, PreferenceId, PartnerId | `src/PromoCodeFactory.WebHost/Models/GivePromoCodeRequest.cs` |
| 1.2 | Добавить метод выдачи промокода в контроллер | Реализовать POST endpoint `/api/v1/promocodes/give` в PromocodesController | `src/PromoCodeFactory.WebHost/Controllers/PromocodesController.cs` |
| 1.3 | Реализовать логику генерации промокода | Добавить метод для создания уникального номера промокода с заданным форматом | `src/PromoCodeFactory.Core/Domain/PromoCodeManagement/PromoCode.cs` | ✅ |
| 1.4 | Добавить проверку существования клиента и предпочтения | Проверить, что клиент и предпочтение существуют в базе данных | `src/PromoCodeFactory.WebHost/Controllers/PromocodesController.cs` | ✅ |
| 1.5 | Добавить создание записи о промокоде в базе данных | Реализовать сохранение промокода с указанием клиента, партнера и предпочтения | `src/PromoCodeFactory.DataAccess/Repositories/EfRepository.cs` | ✅ |
| 1.6 | Написать тесты для метода выдачи промокодов | Добавить xUnit тесты для проверки корректности работы endpoint | `src/PromoCodeFactory.UnitTests/WebHost/Controllers/Promocodes/GivePromoCodeAsyncTests.cs` | ✅ |

## Задача 2: Добавить проверку лимитов партнёров при выдаче промокодов

| № | Подзадача | Описание | Файлы |
|---|-----------|----------|-------|
| 2.1 | Добавить метод для получения активного лимита партнера | Реализовать проверку наличия активного лимита в PartnerPromoCodeLimit | `src/PromoCodeFactory.Core/Domain/PromoCodeManagement/Partner.cs` |
| 2.2 | Добавить проверку превышения лимита в контроллере | Проверить, не превысил ли партнер лимит на выдачу промокодов | `src/PromoCodeFactory.WebHost/Controllers/PromocodesController.cs` |
| 2.3 | Добавить инкремент счетчика выданных промокодов | Увеличивать NumberIssuedPromoCodes у партнера при выдаче промокода | `src/PromoCodeFactory.Core/Domain/PromoCodeManagement/Partner.cs` |
| 2.4 | Написать тесты для проверки лимитов | Добавить тесты для проверки сценариев с превышением лимита | `src/PromoCodeFactory.UnitTests/WebHost/Controllers/Promocodes/CheckPromoCodeLimitTests.cs` |

## Задача 3: Реализовать валидацию данных на уровне домена

| № | Подзадача | Описание | Файлы |
|---|-----------|----------|-------|
| 3.1 | Добавить валидацию в класс Customer | Проверить, что FirstName, LastName и Email не пустые и корректного формата | `src/PromoCodeFactory.Core/Domain/PromoCodeManagement/Customer.cs` |
| 3.2 | Добавить валидацию в класс Employee | Проверить, что FirstName, LastName и Email не пустые | `src/PromoCodeFactory.Core/Domain/Administration/Employee.cs` |
| 3.3 | Добавить валидацию в класс PromoCode | Проверить, что Number и Discount не пустые и положительные | `src/PromoCodeFactory.Core/Domain/PromoCodeManagement/PromoCode.cs` |
| 3.4 | Добавить валидацию в класс Partner | Проверить, что Name не пустое | `src/PromoCodeFactory.Core/Domain/PromoCodeManagement/Partner.cs` |
| 3.5 | Добавить валидацию в модели запросов | Проверить данные на уровне API с помощью атрибутов [Required], [EmailAddress] и т.д. | `src/PromoCodeFactory.WebHost/Models/` |

## Задача 4: Добавить аутентификацию и авторизацию по ролям

| № | Подзадача | Описание | Файлы |
|---|-----------|----------|-------|
| 4.1 | Добавить пакет Microsoft.AspNetCore.Authentication.JwtBearer | Установить NuGet пакет для работы с JWT токенами | `src/PromoCodeFactory.WebHost/PromoCodeFactory.WebHost.csproj` |
| 4.2 | Конфигурация аутентификации в Startup.cs | Добавить сервис AddAuthentication с JwtBearerDefaults.AuthenticationScheme | `src/PromoCodeFactory.WebHost/Startup.cs` |
| 4.3 | Добавить middleware аутентификации | Включить app.UseAuthentication() в Configure методе | `src/PromoCodeFactory.WebHost/Startup.cs` |
| 4.4 | Добавить атрибуты авторизации в контроллерах | Добавить [Authorize] атрибут на контроллеры или методы | `src/PromoCodeFactory.WebHost/Controllers/` |
| 4.5 | Реализовать endpoint для получения токена | Создать AuthController с методом для генерации JWT токена по учетным данным | `src/PromoCodeFactory.WebHost/Controllers/AuthController.cs` |
| 4.6 | Добавить роли в систему | Реализовать логику назначения ролей пользователям и проверку прав | `src/PromoCodeFactory.Core/Domain/Administration/Role.cs` |

## Задача 5: Реализовать глобальную обработку исключений

| № | Подзадача | Описание | Файлы |
|---|-----------|----------|-------|
| 5.1 | Создать класс глобального обработчика ошибок | Реализовать middleware для перехвата и обработки исключений | `src/PromoCodeFactory.WebHost/Middleware/ExceptionMiddleware.cs` |
| 5.2 | Добавить middleware в pipeline | Включить ExceptionMiddleware в Configure методе | `src/PromoCodeFactory.WebHost/Startup.cs` |
| 5.3 | Создать модель ответа об ошибке | Добавить класс ErrorResponse с полями Message, StatusCode, Timestamp | `src/PromoCodeFactory.WebHost/Models/ErrorResponse.cs` |
| 5.4 | Реализовать логирование исключений | Добавить использование ILogger для записи ошибок в файл или сервис логирования | `src/PromoCodeFactory.WebHost/Middleware/ExceptionMiddleware.cs` |

## Задача 6: Добавить покрытие тестами для всех контроллеров

| № | Подзадача | Описание | Файлы |
|---|-----------|----------|-------|
| 6.1 | Написать тесты для CustomersController | Добавить тесты для методов Get, Create, Update, Delete | `src/PromoCodeFactory.UnitTests/WebHost/Controllers/Customers/` |
| 6.2 | Написать тесты для EmployeesController | Добавить тесты для методов Get, Create, Update, Delete | `src/PromoCodeFactory.UnitTests/WebHost/Controllers/Employees/` |
| 6.3 | Написать тесты для PromocodesController | Добавить тесты для методов Get, Give, Cancel | `src/PromoCodeFactory.UnitTests/WebHost/Controllers/Promocodes/` |
| 6.4 | Написать тесты для PreferencesController | Добавить тесты для методов Get, Create, Delete | `src/PromoCodeFactory.UnitTests/WebHost/Controllers/Preferences/` |
| 6.5 | Написать тесты для RolesController | Добавить тесты для методов Get | `src/PromoCodeFactory.UnitTests/WebHost/Controllers/Roles/` |
| 6.6 | Запустить тесты и проверить покрытие | Использовать dotnet test и tools like Coverlet для проверки покрытия | N/A |

## Задача 7: Реализовать фильтрацию, сортировку и пагинацию данных

| № | Подзадача | Описание | Файлы |
|---|-----------|----------|-------|
| 7.1 | Добавить параметры фильтрации в контроллеры | Добавить query параметры для фильтрации по полям | `src/PromoCodeFactory.WebHost/Controllers/` |
| 7.2 | Реализовать логику фильтрации в репозиториях | Добавить методы для фильтрации данных в EfRepository | `src/PromoCodeFactory.DataAccess/Repositories/EfRepository.cs` |
| 7.3 | Добавить параметры сортировки | Добавить query параметры для сортировки по полям (asc/desc) | `src/PromoCodeFactory.WebHost/Controllers/` |
| 7.4 | Реализовать пагинацию | Добавить параметры PageNumber и PageSize для возврата части данных | `src/PromoCodeFactory.WebHost/Controllers/`, `src/PromoCodeFactory.DataAccess/Repositories/EfRepository.cs` |
| 7.5 | Создать модель ответа с пагинацией | Добавить класс PagedResponse<T> с полями Items, TotalCount, PageNumber, PageSize | `src/PromoCodeFactory.WebHost/Models/PagedResponse.cs` |

## Задача 8: Добавить поддержку PostgreSQL для производственной среды

| № | Подзадача | Описание | Файлы |
|---|-----------|----------|-------|
| 8.1 | Добавить пакет Npgsql.EntityFrameworkCore.PostgreSQL | Установить NuGet пакет для работы с PostgreSQL | `src/PromoCodeFactory.DataAccess/PromoCodeFactory.DataAccess.csproj`, `src/PromoCodeFactory.WebHost/PromoCodeFactory.WebHost.csproj` |
| 8.2 | Настроить подключение к PostgreSQL в Startup.cs | Добавить условие для выбора провайдера базы данных в зависимости от конфигурации | `src/PromoCodeFactory.WebHost/Startup.cs` |
| 8.3 | Добавить строки подключения в appsettings.json | Добавить ConnectionString для PostgreSQL в конфигурационные файлы | `src/PromoCodeFactory.WebHost/appsettings.json`, `src/PromoCodeFactory.WebHost/appsettings.Development.json` |
| 8.4 | Проверить работу миграций | Убедиться, что миграции Entity Framework Core работают с PostgreSQL | N/A |

## Задача 9: Рефакторинг кода без изменения поведения

| № | Подзадача | Описание | Файлы |
|---|-----------|----------|-------|
| 9.1 | Улучшить структуру контроллеров | Убедиться, что контроллеры отвечают принципам SOLID, разбить большие методы на маленькие | `src/PromoCodeFactory.WebHost/Controllers/` | ✅ |
| 9.2 | Реорганизовать доменную логику | Переместить бизнес-логику из контроллеров в сервисы уровня приложения | `src/PromoCodeFactory.WebHost/Controllers/`, `src/PromoCodeFactory.Core/ApplicationServices/` |
| 9.3 | Улучшить репозитории | Убедиться, что репозитории следуют шаблону и не содержат бизнес-логику | `src/PromoCodeFactory.DataAccess/Repositories/` |
| 9.4 | Улучшить модель представлений | Убедиться, что модели запросов и ответов корректно структурированы и не дублируют доменные модели | `src/PromoCodeFactory.WebHost/Models/` |
| 9.5 | Улучшить настроенный код | Удалить неиспользуемый код, упростить сложные условия, улучшить читаемость | `src/PromoCodeFactory.WebHost/`, `src/PromoCodeFactory.Core/`, `src/PromoCodeFactory.DataAccess/` |
| 9.6 | Проверить соответствие стандартам кодирования | Убедиться, что код соответствует соглашениям о стиле кодирования C# | `src/PromoCodeFactory.WebHost/`, `src/PromoCodeFactory.Core/`, `src/PromoCodeFactory.DataAccess/` |
