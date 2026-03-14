using System;
using System.Threading.Tasks;
using McpServer.Models;
using McpServer.Services;
using Microsoft.Extensions.Logging;

namespace McpServer.Tools
{
    /// <summary>
    /// Инструмент для удаления клиента. Это ИИ, детка
    /// </summary>
    public class DeleteCustomerTool
    {
        private readonly IPromoCodeFactoryApiClient _apiClient;
        private readonly ILogger<DeleteCustomerTool> _logger;

        public DeleteCustomerTool(
            IPromoCodeFactoryApiClient apiClient,
            ILogger<DeleteCustomerTool> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        /// <summary>
        /// Удаляет клиента
        /// </summary>
        /// <param name="customerId">ID клиента</param>
        /// <returns>Результат операции</returns>
        public async Task<ToolResult<bool>> ExecuteAsync(Guid customerId)
        {
            try
            {
                _logger.LogInformation("Выполнение инструмента удаления клиента: {CustomerId}", customerId);
                
                var result = await _apiClient.DeleteCustomerAsync(customerId);
                
                return new ToolResult<bool>
                {
                    Success = result,
                    Message = result ? "Клиент успешно удален" : "Не удалось удалить клиента",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении инструмента удаления клиента: {CustomerId}", customerId);
                
                return new ToolResult<bool>
                {
                    Success = false,
                    Message = $"Ошибка: {ex.Message}",
                    Data = false
                };
            }
        }
    }
}
