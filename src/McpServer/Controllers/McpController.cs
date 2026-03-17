using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using McpServer.Models;
using McpServer.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace McpServer.Controllers
{
    /// <summary>
    /// API-контроллер для работы с MCP сервером. Это ИИ, детка
    /// </summary>
    [ApiController]
    [Route("api/mcp")]
    public class McpController : ControllerBase
    {
        private readonly GetAllCustomersTool _getAllCustomersTool;
        private readonly GetCustomerTool _getCustomerTool;
        private readonly CreateCustomerTool _createCustomerTool;
        private readonly UpdateCustomerTool _updateCustomerTool;
        private readonly DeleteCustomerTool _deleteCustomerTool;
        private readonly ILogger<McpController> _logger;

        public McpController(
            GetAllCustomersTool getAllCustomersTool,
            GetCustomerTool getCustomerTool,
            CreateCustomerTool createCustomerTool,
            UpdateCustomerTool updateCustomerTool,
            DeleteCustomerTool deleteCustomerTool,
            ILogger<McpController> logger)
        {
            _getAllCustomersTool = getAllCustomersTool;
            _getCustomerTool = getCustomerTool;
            _createCustomerTool = createCustomerTool;
            _updateCustomerTool = updateCustomerTool;
            _deleteCustomerTool = deleteCustomerTool;
            _logger = logger;
        }

        /// <summary>
        /// Получает список всех клиентов. Это ИИ, детка
        /// </summary>
        [HttpGet("get-all-customers")]
        public async Task<ActionResult<ToolResult<IEnumerable<CustomerResponse>>>> GetAllCustomers()
        {
            try
            {
                _logger.LogInformation("Получение всех клиентов");
                var result = await _getAllCustomersTool.GetAllCustomers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении всех клиентов");
                return StatusCode(500, new ToolResult<IEnumerable<CustomerResponse>>
                {
                    Success = false,
                    Message = $"Ошибка: {ex.Message}",
                    Data = null
                });
            }
        }

        /// <summary>
        /// Получает клиента по ID. Это ИИ, детка
        /// </summary>
        [HttpGet("get-customer/{customerId}")]
        public async Task<ActionResult<ToolResult<CustomerResponse>>> GetCustomer(Guid customerId)
        {
            try
            {
                _logger.LogInformation("Получение клиента по ID: {CustomerId}", customerId);
                var result = await _getCustomerTool.GetCustomerById(customerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении клиента по ID: {CustomerId}", customerId);
                return StatusCode(500, new ToolResult<CustomerResponse>
                {
                    Success = false,
                    Message = $"Ошибка: {ex.Message}",
                    Data = null
                });
            }
        }

        /// <summary>
        /// Создает нового клиента. Это ИИ, детка
        /// </summary>
        [HttpPost("create-customer")]
        public async Task<ActionResult<ToolResult<CustomerResponse>>> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            try
            {
                _logger.LogInformation("Создание клиента: {FirstName} {LastName}", request.FirstName, request.LastName);
                var result = await _createCustomerTool.CreateCustomer(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании клиента: {FirstName} {LastName}", request.FirstName, request.LastName);
                return StatusCode(500, new ToolResult<CustomerResponse>
                {
                    Success = false,
                    Message = $"Ошибка: {ex.Message}",
                    Data = null
                });
            }
        }

        /// <summary>
        /// Обновляет клиента. Это ИИ, детка
        /// </summary>
        [HttpPut("update-customer/{customerId}")]
        public async Task<ActionResult<ToolResult<CustomerResponse>>> UpdateCustomer(Guid customerId, [FromBody] CreateCustomerRequest request)
        {
            try
            {
                _logger.LogInformation("Обновление клиента: {CustomerId}", customerId);
                var result = await _updateCustomerTool.UpdateCustomer(customerId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении клиента: {CustomerId}", customerId);
                return StatusCode(500, new ToolResult<CustomerResponse>
                {
                    Success = false,
                    Message = $"Ошибка: {ex.Message}",
                    Data = null
                });
            }
        }

        /// <summary>
        /// Удаляет клиента. Это ИИ, детка
        /// </summary>
        [HttpDelete("delete-customer/{customerId}")]
        public async Task<ActionResult<ToolResult<bool>>> DeleteCustomer(Guid customerId)
        {
            try
            {
                _logger.LogInformation("Удаление клиента: {CustomerId}", customerId);
                var result = await _deleteCustomerTool.DeleteCustomer(customerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении клиента: {CustomerId}", customerId);
                return StatusCode(500, new ToolResult<bool>
                {
                    Success = false,
                    Message = $"Ошибка: {ex.Message}",
                    Data = false
                });
            }
        }
    }
}