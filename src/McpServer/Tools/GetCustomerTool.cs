using System;
using System.Threading.Tasks;
using McpServer.Models;
using McpServer.Services;
using Microsoft.Extensions.Logging;

namespace McpServer.Tools
{
    /// <summary>
    /// Инструмент для получения клиента по ID. Это ИИ, детка
    /// </summary>
    public class GetCustomerTool
    {
        private readonly IPromoCodeFactoryApiClient _apiClient;
        private readonly ILogger<GetCustomerTool> _logger;

        public GetCustomerTool(
            IPromoCodeFactoryApiClient apiClient,
            ILogger<GetCustomerTool> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        /// <summary>
        /// Получает клиента по ID
        /// </summary>
        /// <param name="customerId">ID клиента</param>
        /// <returns>Результат операции</returns>
        public async Task<ToolResult<CustomerResponse>> ExecuteAsync(Guid customerId)
        {
            try
            {
                _logger.LogInformation("Выполнение инструмента получения клиента по ID: {CustomerId}", customerId);
                
                var customer = await _apiClient.GetCustomerByIdAsync(customerId);
                
                if (customer == null)
                {
                    return new ToolResult<CustomerResponse>
                    {
                        Success = false,
                        Message = "Клиент не найден",
                        Data = null
                    };
                }
                
                return new ToolResult<CustomerResponse>
                {
                    Success = true,
                    Message = "Клиент успешно найден",
                    Data = customer
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении инструмента получения клиента по ID: {CustomerId}", customerId);
                
                return new ToolResult<CustomerResponse>
                {
                    Success = false,
                    Message = $"Ошибка: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
