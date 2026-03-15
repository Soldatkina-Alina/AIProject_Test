using McpServer.Models;
using McpServer.Tools;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace McpServer.Controllers
{
    [Route("api/mcp")]
    [ApiController]
    public class McpController : ControllerBase
    {
        private readonly CreateCustomerTool _createCustomerTool;
        private readonly GetCustomerTool _getCustomerTool;
        private readonly GetAllCustomersTool _getAllCustomersTool;
        private readonly UpdateCustomerTool _updateCustomerTool;
        private readonly DeleteCustomerTool _deleteCustomerTool;

        public McpController(
            CreateCustomerTool createCustomerTool,
            GetCustomerTool getCustomerTool,
            GetAllCustomersTool getAllCustomersTool,
            UpdateCustomerTool updateCustomerTool,
            DeleteCustomerTool deleteCustomerTool)
        {
            _createCustomerTool = createCustomerTool;
            _getCustomerTool = getCustomerTool;
            _getAllCustomersTool = getAllCustomersTool;
            _updateCustomerTool = updateCustomerTool;
            _deleteCustomerTool = deleteCustomerTool;
        }

        [HttpGet("get-all-customers")]
        public async Task<IActionResult> GetAllCustomersAsync()
        {
            var result = await _getAllCustomersTool.ExecuteAsync();
            return Ok(result);
        }

        [HttpPost("create-customer")]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateCustomerRequest request)
        {
            var result = await _createCustomerTool.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpGet("get-customer/{customerId}")]
        public async Task<IActionResult> GetCustomerAsync(Guid customerId)
        {
            var result = await _getCustomerTool.ExecuteAsync(customerId);
            return Ok(result);
        }

        [HttpPut("update-customer/{customerId}")]
        public async Task<IActionResult> UpdateCustomerAsync(Guid customerId, [FromBody] CreateCustomerRequest request)
        {
            var result = await _updateCustomerTool.ExecuteAsync(customerId, request);
            return Ok(result);
        }

        [HttpDelete("delete-customer/{customerId}")]
        public async Task<IActionResult> DeleteCustomerAsync(Guid customerId)
        {
            var result = await _deleteCustomerTool.ExecuteAsync(customerId);
            return Ok(result);
        }
    }
}