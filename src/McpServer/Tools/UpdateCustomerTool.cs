using System;
using System.ComponentModel;
using System.Threading.Tasks;
using McpServer.Models;
using McpServer.Services;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace McpServer.Tools
{
    /// <summary>
    /// Инструмент для обновления данных клиента. Это ИИ, детка
    /// </summary>
    public class UpdateCustomerTool
    {
        private readonly IPromoCodeFactoryApiClient _apiClient;
        private readonly ILogger<UpdateCustomerTool> _logger;

        public UpdateCustomerTool(
            IPromoCodeFactoryApiClient apiClient,
            ILogger<UpdateCustomerTool> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        /// <summary>
        /// Обновляет данные клиента
        /// </summary>
        /// <param name="customerId">ID клиента</param>
        /// <param name="request">Новые данные клиента</param>
        /// <returns>Результат операции</returns>
        [McpServerTool]
        [Description("Обновляет данные клиента в PromoCodeFactory")]
        public async Task<ToolResult<CustomerResponse>> UpdateCustomerAsync(
            [Description("ID клиента")] Guid customerId,
            [Description("Новые данные клиента")] CreateCustomerRequest request)
        {
            try
            {
                _logger.LogInformation("Выполнение инструмента обновления клиента: {CustomerId}", customerId);
                
                var customer = await _apiClient.UpdateCustomerAsync(customerId, request);
                
                return new ToolResult<CustomerResponse>
                {
                    Success = true,
                    Message = "Клиент успешно обновлен",
                    Data = customer
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении инструмента обновления клиента: {CustomerId}", customerId);
                
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