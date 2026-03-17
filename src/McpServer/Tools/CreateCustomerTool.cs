using System;
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
    /// Инструмент для создания клиента в PromoCodeFactory. Это ИИ, детка
    /// </summary>
    public class CreateCustomerTool
    {
        private readonly IPromoCodeFactoryApiClient _apiClient;
        private readonly ILogger<CreateCustomerTool> _logger;

        public CreateCustomerTool(
            IPromoCodeFactoryApiClient apiClient,
            ILogger<CreateCustomerTool> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        /// <summary>
        /// Создает нового клиента
        /// </summary>
        /// <param name="request">Данные для создания клиента</param>
        /// <returns>Результат операции</returns>
        [McpServerTool]
        [Description("Создает нового клиента в PromoCodeFactory")]
        public async Task<ToolResult<CustomerResponse>> CreateCustomerAsync(
            [Description("Данные для создания клиента")] CreateCustomerRequest request)
        {
            try
            {
                _logger.LogInformation("Выполнение инструмента создания клиента: {FirstName} {LastName}", request.FirstName, request.LastName);
                
                // Если PreferenceIds пустой, получаем первый предпочтение из БД
                if (request.PreferenceIds == null || request.PreferenceIds.Count == 0)
                {
                    _logger.LogInformation("PreferenceIds пустой, получаем первый предпочтение из БД");
                    var preferences = await _apiClient.GetPreferencesAsync();
                    
                    if (preferences != null)
                    {
                        foreach (var preference in preferences)
                        {
                            request.PreferenceIds.Add(preference.Id);
                            _logger.LogInformation("Добавлено предпочтение: {PreferenceName} (ID: {PreferenceId})", preference.Name, preference.Id);
                            break; // Берём только первый
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Не удалось получить список предпочтений");
                    }
                }
                
                var customer = await _apiClient.CreateCustomerAsync(request);
                
                return new ToolResult<CustomerResponse>
                {
                    Success = true,
                    Message = "Клиент успешно создан",
                    Data = customer
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении инструмента создания клиента");
                
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