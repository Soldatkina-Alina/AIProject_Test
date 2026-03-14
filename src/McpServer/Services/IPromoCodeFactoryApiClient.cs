using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using McpServer.Models;

namespace McpServer.Services
{
    /// <summary>
    /// Интерфейс для взаимодействия с PromoCodeFactory API. Это ИИ, детка
    /// </summary>
    public interface IPromoCodeFactoryApiClient
    {
        /// <summary>
        /// Получает список всех предпочтений
        /// </summary>
        /// <returns>Список предпочтений</returns>
        Task<IEnumerable<PreferenceResponse>> GetPreferencesAsync();

        /// <summary>
        /// Получает список всех клиентов
        /// </summary>
        /// <returns>Список клиентов</returns>
        Task<IEnumerable<CustomerResponse>> GetCustomersAsync();

        /// <summary>
        /// Получает клиента по ID
        /// </summary>
        /// <param name="customerId">ID клиента</param>
        /// <returns>Данные клиента</returns>
        Task<CustomerResponse> GetCustomerByIdAsync(Guid customerId);

        /// <summary>
        /// Создает нового клиента
        /// </summary>
        /// <param name="request">Данные для создания клиента</param>
        /// <returns>Созданный клиент</returns>
        Task<CustomerResponse> CreateCustomerAsync(CreateCustomerRequest request);

        /// <summary>
        /// Обновляет данные клиента
        /// </summary>
        /// <param name="customerId">ID клиента</param>
        /// <param name="request">Новые данные клиента</param>
        /// <returns>Обновленные данные клиента</returns>
        Task<CustomerResponse> UpdateCustomerAsync(Guid customerId, CreateCustomerRequest request);

        /// <summary>
        /// Удаляет клиента
        /// </summary>
        /// <param name="customerId">ID клиента</param>
        /// <returns>Результат операции</returns>
        Task<bool> DeleteCustomerAsync(Guid customerId);
    }
}
