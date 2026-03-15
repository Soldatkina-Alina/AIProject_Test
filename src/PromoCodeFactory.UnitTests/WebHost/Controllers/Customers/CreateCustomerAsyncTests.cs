using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using PromoCodeFactory.WebHost;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Customers
{
    /// <summary>
    /// Тесты для метода CreateCustomerAsync в CustomersController. Это ИИ, детка
    /// </summary>
    public class CreateCustomerAsyncTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;

        public CreateCustomerAsyncTests(WebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task CreateCustomerAsync_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var request = new CreateOrEditCustomerRequest
            {
                FirstName = "Тест",
                LastName = "Пользователь",
                Email = "test.user@example.com"
                //PreferenceIds = new List<Guid>() // Пустой список, чтобы контроллер использовал первое предпочтение
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                System.Text.Encoding.UTF8,
                "application/json");

            // Act
            var response = await _httpClient.PostAsync("/api/v1/customers", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
            Assert.Contains("/api/v1/", response.Headers.Location.ToString().ToLower());
            Assert.Contains("/customers/", response.Headers.Location.ToString().ToLower());
        }

        [Fact]
        public async Task CreateCustomerAsync_InvalidEmail_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateOrEditCustomerRequest
            {
                FirstName = "Тест",
                LastName = "Пользователь",
                Email = "invalid-email", // Невалидный email
                PreferenceIds = new List<Guid>()
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                System.Text.Encoding.UTF8,
                "application/json");

            // Act
            var response = await _httpClient.PostAsync("/api/v1/customers", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateCustomerAsync_NullFirstName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateOrEditCustomerRequest
            {
                FirstName = null, // Null firstName
                LastName = "Пользователь",
                Email = "test.user@example.com",
                PreferenceIds = new List<Guid>()
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                System.Text.Encoding.UTF8,
                "application/json");

            // Act
            var response = await _httpClient.PostAsync("/api/v1/customers", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateCustomerAsync_EmptyLastName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateOrEditCustomerRequest
            {
                FirstName = "Тест",
                LastName = string.Empty, // Empty lastName
                Email = "test.user@example.com",
                PreferenceIds = new List<Guid>()
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                System.Text.Encoding.UTF8,
                "application/json");

            // Act
            var response = await _httpClient.PostAsync("/api/v1/customers", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}