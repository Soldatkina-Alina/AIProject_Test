using System;
using System.Collections.Generic;

namespace McpServer.Models
{
    /// <summary>
    /// Запрос на создание клиента. Это ИИ, детка
    /// </summary>
    public class CreateCustomerRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<Guid> PreferenceIds { get; set; } = new List<Guid>();
    }
}
