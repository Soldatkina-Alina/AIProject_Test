using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Промокоды. Это ИИ, детка
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly IRepository<PromoCode> _promoCodesRepository;
        private readonly IRepository<Customer> _customersRepository;
        private readonly IRepository<Partner> _partnersRepository;
        private readonly IRepository<Preference> _preferencesRepository;

        public PromocodesController(
            IRepository<PromoCode> promoCodesRepository,
            IRepository<Customer> customersRepository,
            IRepository<Partner> partnersRepository,
            IRepository<Preference> preferencesRepository)
        {
            _promoCodesRepository = promoCodesRepository;
            _customersRepository = customersRepository;
            _partnersRepository = partnersRepository;
            _preferencesRepository = preferencesRepository;
        }
        
        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns>Список всех промокодов</returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promoCodes = await _promoCodesRepository.GetAllAsync();

            var response = promoCodes.Select(x => new PromoCodeShortResponse()
            {
                Id = x.Id,
                Code = x.Code,
                BeginDate = x.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                PartnerName = x.PartnerName,
                ServiceInfo = x.ServiceInfo
            }).ToList();

            return Ok(response);
        }
        
        /// <summary>
        /// Выдать промокод клиенту
        /// </summary>
        /// <param name="request">Данные запроса на выдачу промокода</param>
        /// <returns>Результат операции</returns>
        [HttpPost("give")]
        public async Task<IActionResult> GivePromoCodeAsync(GivePromoCodeRequest request)
        {
            // Проверка наличия клиента
            var customer = await _customersRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
            {
                return NotFound("Клиент не найден");
            }

            // Проверка наличия партнера
            var partner = await _partnersRepository.GetByIdAsync(request.PartnerId);
            if (partner == null)
            {
                return NotFound("Партнер не найден");
            }

            // Проверка наличия предпочтения
            var preference = await _preferencesRepository.GetByIdAsync(request.PreferenceId);
            if (preference == null)
            {
                return NotFound("Предпочтение не найдено");
            }

            // Проверка, что клиент имеет указанное предпочтение
            var customerPreference = customer.Preferences.FirstOrDefault(p => p.PreferenceId == request.PreferenceId);
            if (customerPreference == null)
            {
                return BadRequest("У клиента нет указанного предпочтения");
            }

            // Создание нового промокода
            var newPromoCode = PromoCode.Create(
                partner.Name,
                preference,
                partner.PartnerManager
            );

            // Сохранение промокода в базе данных
            await _promoCodesRepository.AddAsync(newPromoCode);

            // TODO: Добавить связь промокода с клиентом (если требуется)

            return Ok("Промокод успешно выдан");
        }
    }
}
