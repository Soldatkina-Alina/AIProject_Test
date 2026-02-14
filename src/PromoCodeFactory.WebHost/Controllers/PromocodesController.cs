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
            var response = MapPromoCodesToShortResponses(promoCodes);
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
            var validationResult = await ValidateRequest(request);
            if (validationResult != null)
            {
                return validationResult;
            }

            var customer = await _customersRepository.GetByIdAsync(request.CustomerId);
            var partner = await _partnersRepository.GetByIdAsync(request.PartnerId);
            var preference = await _preferencesRepository.GetByIdAsync(request.PreferenceId);

            var newPromoCode = CreatePromoCode(partner, preference);
            await SavePromoCode(newPromoCode);

            return Ok("Промокод успешно выдан");
        }

        private List<PromoCodeShortResponse> MapPromoCodesToShortResponses(IEnumerable<PromoCode> promoCodes)
        {
            return promoCodes.Select(x => new PromoCodeShortResponse()
            {
                Id = x.Id,
                Code = x.Code,
                BeginDate = x.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                PartnerName = x.PartnerName,
                ServiceInfo = x.ServiceInfo
            }).ToList();
        }

        private async Task<IActionResult> ValidateRequest(GivePromoCodeRequest request)
        {
            if (!await CustomerExists(request.CustomerId))
            {
                return NotFound("Клиент не найден");
            }

            if (!await PartnerExists(request.PartnerId))
            {
                return NotFound("Партнер не найден");
            }

            if (!await PreferenceExists(request.PreferenceId))
            {
                return NotFound("Предпочтение не найдено");
            }

            if (!await CustomerHasPreference(request.CustomerId, request.PreferenceId))
            {
                return BadRequest("У клиента нет указанного предпочтения");
            }

            return null;
        }

        private async Task<bool> CustomerExists(Guid customerId)
        {
            var customer = await _customersRepository.GetByIdAsync(customerId);
            return customer != null;
        }

        private async Task<bool> PartnerExists(Guid partnerId)
        {
            var partner = await _partnersRepository.GetByIdAsync(partnerId);
            return partner != null;
        }

        private async Task<bool> PreferenceExists(Guid preferenceId)
        {
            var preference = await _preferencesRepository.GetByIdAsync(preferenceId);
            return preference != null;
        }

        private async Task<bool> CustomerHasPreference(Guid customerId, Guid preferenceId)
        {
            var customer = await _customersRepository.GetByIdAsync(customerId);
            var customerPreference = customer.Preferences.FirstOrDefault(p => p.PreferenceId == preferenceId);
            return customerPreference != null;
        }

        private PromoCode CreatePromoCode(Partner partner, Preference preference)
        {
            return PromoCode.Create(
                partner.Name,
                preference,
                partner.PartnerManager
            );
        }

        private async Task SavePromoCode(PromoCode promoCode)
        {
            await _promoCodesRepository.AddAsync(promoCode);
        }
    }
}
