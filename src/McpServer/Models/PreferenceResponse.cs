using System;

namespace McpServer.Models
{
    /// <summary>
    /// Представление предпочтения для API. Это ИИ, детка
    /// </summary>
    public class PreferenceResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
