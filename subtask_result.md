## Подзадача: Запуск контейнеров и подключение MCP сервера к Cline

### Промт
Прочти 'README.md' , запусти контейр в докере. А затем попробуй подключить mcp server к cline. Выяви в чем проблемы

### Проверенные файлы
- README.md
- compose.yml
- src/docker-compose.yml
- src/McpServer/Dockerfile
- src/PromoCodeFactory.WebHost/Dockerfile
- src/McpServer/Controllers/McpController.cs
- src/McpServer/Program.cs
- src/McpServer/Startup.cs
- src/McpServer/McpServer.csproj
- ../../../AppData/Roaming/Code/User/globalStorage/saoudrizwan.claude-dev/settings/cline_mcp_settings.json
- ../../../source/repos/McpRag/McpRag/Program.cs
- ../../../source/repos/McpRag/McpRag/Tools/EchoTools.cs

### Результаты выполнения
1. **Контейнеры запущены успешно**:
   - PromoCodeFactory.WebHost (порт 5001:8080)
   - McpServer (порт 5002:8080)

2. **Проблема с подключением к MCP серверу**:
   - Cline не может подключиться к McpServer с ошибкой "Not connected"
   - При прямом HTTP запросе к http://localhost:5002/api/mcp возвращается 404 Not Found

3. **Основная причина проблемы**:
   - McpServer реализован как обычное ASP.NET Core приложение с REST API контроллерами
   - Для Cline нужен MCP сервер, реализованный с помощью Model Context Protocol SDK (как McpRag)
   - В McpServer отсутствуют:
     - Зависимости от MCP Server SDK
     - Атрибуты [McpServerTool] у инструментов
     - Настройка с AddMcpServer() в Program.cs
     - Выбор транспорта (stdio или http)

### Запущенные контейнеры
```bash
NAME                     IMAGE                 COMMAND                  STATUS          PORTS
src-mcpserver-1          src-mcpserver         "dotnet McpServer.dll"   Up 10 seconds   0.0.0.0:5002->8080/tcp
src-promocodefactory-1   src-promocodefactory  "dotnet PromoCodeFac…"   Up 11 seconds   0.0.0.0:5001->8080/tcp