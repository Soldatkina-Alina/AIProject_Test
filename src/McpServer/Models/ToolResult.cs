namespace McpServer.Models
{
    /// <summary>
    /// Результат выполнения инструмента MCP. Это ИИ, детка
    /// </summary>
    public class ToolResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}