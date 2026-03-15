using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using McpServer.Models;
using Xunit;

namespace McpServer.IntegrationTests
{
    public class McpControllerIntegrationTests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5002/api/mcp";
        JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // для игнорирования регистра
        };

        public McpControllerIntegrationTests()
        {
            _httpClient = new HttpClient();
        }

        [Fact]
        public async Task CreateCustomer_ShouldReturnSuccess()
        {
            // Arrange
            var request = new CreateCustomerRequest
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com"
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PostAsync($"{_baseUrl}/create-customer", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ToolResult<string>>(responseContent, _jsonOptions);
            Assert.NotNull(result);
            Assert.Contains("успешно", result.Message);
        }

        [Fact]
        public async Task GetCustomer_ShouldReturnSuccess()
        {
            // Arrange - сначала создаем клиента для теста
            var createRequest = new CreateCustomerRequest
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com"
            };
            var createJson = JsonSerializer.Serialize(createRequest);
            var createContent = new StringContent(createJson, Encoding.UTF8, "application/json");
            var createResponse = await _httpClient.PostAsync($"{_baseUrl}/create-customer", createContent);
            createResponse.EnsureSuccessStatusCode();

            var createResult = JsonSerializer.Deserialize<ToolResult<string>>(await createResponse.Content.ReadAsStringAsync(), _jsonOptions);
            var customerId = Guid.Parse(createResult.Data.ToString());

            // Act
            var response = await _httpClient.GetAsync($"{_baseUrl}/get-customer/{customerId}");
           
            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ToolResult<CustomerResponse>>(responseContent, _jsonOptions);
            Assert.NotNull(result);
            Assert.Contains("успешно", result.Message);
        }

        [Fact]
        public async Task UpdateCustomer_ShouldReturnSuccess()
        {
            // Arrange - сначала создаем клиента для теста
            var createRequest = new CreateCustomerRequest
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com"
            };
            var createJson = JsonSerializer.Serialize(createRequest);
            var createContent = new StringContent(createJson, Encoding.UTF8, "application/json");
            var createResponse = await _httpClient.PostAsync($"{_baseUrl}/create-customer", createContent);
            createResponse.EnsureSuccessStatusCode();
            
            var createResult = JsonSerializer.Deserialize<ToolResult<string>>(await createResponse.Content.ReadAsStringAsync(), _jsonOptions);
            var customerId = Guid.Parse(createResult.Data.ToString());

            var updateRequest = new CreateCustomerRequest
            {
                FirstName = "Updated",
                LastName = "User",
                Email = "updated@example.com"
            };
            var updateJson = JsonSerializer.Serialize(updateRequest);
            var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PutAsync($"{_baseUrl}/update-customer/{customerId}", updateContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ToolResult<string>>(responseContent, _jsonOptions);
            Assert.NotNull(result);
            Assert.Contains("успешно", result.Message);
        }

        [Fact]
        public async Task DeleteCustomer_ShouldReturnSuccess()
        {
            // Arrange - сначала создаем клиента для теста
            var createRequest = new CreateCustomerRequest
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com"
            };
            var createJson = JsonSerializer.Serialize(createRequest);
            var createContent = new StringContent(createJson, Encoding.UTF8, "application/json");
            var createResponse = await _httpClient.PostAsync($"{_baseUrl}/create-customer", createContent);
            createResponse.EnsureSuccessStatusCode();
            
            var createResult = JsonSerializer.Deserialize<ToolResult<string>>(await createResponse.Content.ReadAsStringAsync(), _jsonOptions);
            var customerId = Guid.Parse(createResult.Data.ToString());

            // Act
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/delete-customer/{customerId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ToolResult<string>>(responseContent, _jsonOptions);
            Assert.NotNull(result);
            Assert.Contains("успешно", result.Message);
        }

        [Fact]
        public async Task GetAllCustomers_ShouldReturnSuccess()
        {
            // Act
            var response = await _httpClient.GetAsync($"{_baseUrl}/get-all-customers");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ToolResult<List<CustomerResponse>>>(responseContent, _jsonOptions);
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Contains("Клиенты успешно получены", result.Message);
            Assert.NotNull(result.Data);
        }
    }
}
