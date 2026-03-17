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
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;

        public CustomersController(IRepository<Customer> customerRepository, 
            IRepository<Preference> preferenceRepository)
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<CustomerShortResponse>>> GetCustomersAsync()
        {
            var customers =  await _customerRepository.GetAllAsync();

            var response = customers.Select(x => new CustomerShortResponse()
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName
            }).ToList();

            return Ok(response);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer =  await _customerRepository.GetByIdAsync(id);

            var response = new CustomerResponse(customer);

            return Ok(response);
        }
        
        [HttpPost]
        public async Task<ActionResult<CustomerResponse>> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = new Customer()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
            
            // Проверяем, что все PreferenceIds являются допустимыми GUID
            bool allIdsValid = true;
            if (request.PreferenceIds != null && request.PreferenceIds.Any())
            {
                foreach (var id in request.PreferenceIds)
                {
                    if (id == Guid.Empty)
                    {
                        allIdsValid = false;
                        break;
                    }
                }
            }
            
            // Проверяем, есть ли предпочтения в запросе и все они допустимы
            if (request.PreferenceIds == null || !request.PreferenceIds.Any() || !allIdsValid)
            {
                // Получаем первое доступное предпочтение из БД
                var allPreferences = await _preferenceRepository.GetAllAsync();
                var firstPreference = allPreferences.FirstOrDefault();
                
                if (firstPreference != null)
                {
                    // Используем первое предпочтение
                    customer.Preferences = new List<CustomerPreference>
                    {
                        new CustomerPreference
                        {
                            Customer = customer,
                            Preference = firstPreference
                        }
                    };
                }
                else
                {
                    // Если нет предпочтений в БД, оставляем пустым
                    customer.Preferences = new List<CustomerPreference>();
                }
            }
            else
            {
                // Получаем предпочтения из бд и сохраняем большой объект
                var preferences = await _preferenceRepository
                    .GetRangeByIdsAsync(request.PreferenceIds);

                customer.Preferences = preferences.Select(x => new CustomerPreference()
                {
                    Customer = customer,
                    Preference = x
                }).ToList();
            }
            
            await _customerRepository.AddAsync(customer);

            var response = new CustomerResponse(customer);
            return CreatedAtAction(nameof(GetCustomerAsync), new {id = customer.Id}, response);
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CustomerResponse>> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            
            if (customer == null)
                return NotFound();
            
            var preferences = await _preferenceRepository.GetRangeByIdsAsync(request.PreferenceIds);
            
            customer.Email = request.Email;
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Preferences.Clear();
            customer.Preferences = preferences.Select(x => new CustomerPreference()
            {
                Customer = customer,
                Preference = x
            }).ToList();

            await _customerRepository.UpdateAsync(customer);

            var response = new CustomerResponse(customer);
            return Ok(response);
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<string>> DeleteCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            
            if (customer == null)
                return NotFound();

            await _customerRepository.DeleteAsync(customer);

            return Ok("Customer deleted successfully");
        }
    }
}