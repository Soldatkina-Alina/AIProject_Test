using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using McpServer.Models;
using McpServer.Services;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace McpServer.Tools
{
    /// <summary>
    /// Инструмент для получения всех клиентов из PromoCodeFactory. Это ИИ, детка
    /// </summary>
    public class GetAllCustomersTool
    {
        private readonly IPromoCodeFactoryApiClient _apiClient;
        private readonly ILogger<GetAllCustomersTool> _logger;

        public GetAllCustomersTool(
            IPromoCodeFactoryApiClient apiClient,
            ILogger<GetAllCustomersTool> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        /// <summary>
        /// Получает список всех клиентов
        /// </summary>
        /// <returns>Результат операции со списком клиентов</returns>
        [McpServerTool]
        [Description("Получает список всех клиентов из PromoCodeFactory")]
        public async Task<ToolResult<IEnumerable<CustomerResponse>>> GetAllCustomers()
        {
            try
            {
                _logger.LogInformation("Выполнение инструмента получения всех клиентов");
                
                var customers = await _apiClient.GetCustomersAsync();
                
                return new ToolResult<IEnumerable<CustomerResponse>>
                {
                    Success = true,
                    Message = "Клиенты успешно получены",
                    Data = customers
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении инструмента получения всех клиентов");
                
                return new ToolResult<IEnumerable<CustomerResponse>>
                {
                    Success = false,
                    Message = $"Ошибка: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}