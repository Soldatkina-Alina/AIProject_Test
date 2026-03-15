using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using McpServer.Models;
using Microsoft.Extensions.Logging;

namespace McpServer.Services
{
    /// <summary>
    /// Клиент для взаимодействия с PromoCodeFactory API. Это ИИ, детка
    /// </summary>
    public class PromoCodeFactoryApiClient : IPromoCodeFactoryApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PromoCodeFactoryApiClient> _logger;

        public PromoCodeFactoryApiClient(
            IHttpClientFactory httpClientFactory,
            ILogger<PromoCodeFactoryApiClient> logger)
        {
            _httpClient = httpClientFactory.CreateClient("PromoCodeFactoryApi");
            _logger = logger;
        }

        public async Task<IEnumerable<PreferenceResponse>> GetPreferencesAsync()
        {
            try
            {
                _logger.LogInformation("Получение списка предпочтений");
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<PreferenceResponse>>("api/v1/preferences");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка предпочтений");
                throw;
            }
        }

        public async Task<IEnumerable<CustomerResponse>> GetCustomersAsync()
        {
            try
            {
                _logger.LogInformation("Получение списка клиентов");
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<CustomerResponse>>("api/v1/customers");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка клиентов");
                throw;
            }
        }

        public async Task<CustomerResponse> GetCustomerByIdAsync(Guid customerId)
        {
            try
            {
                _logger.LogInformation("Получение клиента по ID: {CustomerId}", customerId);
                var response = await _httpClient.GetFromJsonAsync<CustomerResponse>($"api/v1/customers/{customerId}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении клиента по ID: {CustomerId}", customerId);
                throw;
            }
        }

        public async Task<CustomerResponse> CreateCustomerAsync(CreateCustomerRequest request)
        {
            try
            {
                _logger.LogInformation("Создание клиента: {FirstName} {LastName}", request.FirstName, request.LastName);
                var response = await _httpClient.PostAsJsonAsync("api/v1/customers", request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<CustomerResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании клиента: {FirstName} {LastName}", request.FirstName, request.LastName);
                throw;
            }
        }

        public async Task<CustomerResponse> UpdateCustomerAsync(Guid customerId, CreateCustomerRequest request)
        {
            try
            {
                _logger.LogInformation("Обновление клиента: {CustomerId}", customerId);
                var response = await _httpClient.PutAsJsonAsync($"api/v1/customers/{customerId}", request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<CustomerResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении клиента: {CustomerId}", customerId);
                throw;
            }
        }

        public async Task<bool> DeleteCustomerAsync(Guid customerId)
        {
            try
            {
                _logger.LogInformation("Удаление клиента: {CustomerId}", customerId);
                var response = await _httpClient.DeleteAsync($"api/v1/customers/{customerId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении клиента: {CustomerId}", customerId);
                throw;
            }
        }
    }
}
