using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Controllers;
using PromoCodeFactory.WebHost.Models;
using Xunit;
using FluentAssertions;
using AutoFixture;
using AutoFixture.AutoMoq;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Promocodes
{
    /// <summary>
    /// Тесты для метода GivePromoCodeAsync. Это ИИ, детка
    /// </summary>
    public class GivePromoCodeAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRepository<PromoCode>> _promoCodesRepositoryMock;
        private readonly Mock<IRepository<Customer>> _customersRepositoryMock;
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
        private readonly Mock<IRepository<Preference>> _preferencesRepositoryMock;
        private readonly PromocodesController _controller;

        public GivePromoCodeAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _promoCodesRepositoryMock = new Mock<IRepository<PromoCode>>();
            _customersRepositoryMock = new Mock<IRepository<Customer>>();
            _partnersRepositoryMock = new Mock<IRepository<Partner>>();
            _preferencesRepositoryMock = new Mock<IRepository<Preference>>();

            _controller = new PromocodesController(
                _promoCodesRepositoryMock.Object,
                _customersRepositoryMock.Object,
                _partnersRepositoryMock.Object,
                _preferencesRepositoryMock.Object
            );
        }

        /// <summary>
        /// Должен вернуть Ok при успешной выдаче промокода
        /// </summary>
        [Fact]
        public async Task GivePromoCodeAsync_ShouldReturnOk_WhenPromoCodeIsGivenSuccessfully()
        {
            // Arrange
            var request = _fixture.Create<GivePromoCodeRequest>();
            var customer = CreateCustomerWithPreference(request.CustomerId, request.PreferenceId);
            var partner = CreatePartner(request.PartnerId);
            var preference = CreatePreference(request.PreferenceId);

            SetupRepositories(customer, partner, preference);

            // Act
            var result = await _controller.GivePromoCodeAsync(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().Be("Промокод успешно выдан");
        }

        /// <summary>
        /// Должен вернуть NotFound при несуществующем клиенте
        /// </summary>
        [Fact]
        public async Task GivePromoCodeAsync_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            var request = _fixture.Create<GivePromoCodeRequest>();
            _customersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(request.CustomerId))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.GivePromoCodeAsync(request);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = (NotFoundObjectResult)result;
            notFoundResult.Value.Should().Be("Клиент не найден");
        }

        /// <summary>
        /// Должен вернуть NotFound при несуществующем партнере
        /// </summary>
        [Fact]
        public async Task GivePromoCodeAsync_ShouldReturnNotFound_WhenPartnerDoesNotExist()
        {
            // Arrange
            var request = _fixture.Create<GivePromoCodeRequest>();
            var customer = CreateCustomerWithPreference(request.CustomerId, request.PreferenceId);
            var preference = CreatePreference(request.PreferenceId);

            _customersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(request.CustomerId))
                .ReturnsAsync(customer);

            _partnersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(request.PartnerId))
                .ReturnsAsync((Partner)null);

            _preferencesRepositoryMock
                .Setup(repo => repo.GetByIdAsync(request.PreferenceId))
                .ReturnsAsync(preference);

            // Act
            var result = await _controller.GivePromoCodeAsync(request);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = (NotFoundObjectResult)result;
            notFoundResult.Value.Should().Be("Партнер не найден");
        }

        /// <summary>
        /// Должен вернуть NotFound при несуществующем предпочтении
        /// </summary>
        [Fact]
        public async Task GivePromoCodeAsync_ShouldReturnNotFound_WhenPreferenceDoesNotExist()
        {
            // Arrange
            var request = _fixture.Create<GivePromoCodeRequest>();
            var customer = CreateCustomerWithPreference(request.CustomerId, request.PreferenceId);
            var partner = CreatePartner(request.PartnerId);

            _customersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(request.CustomerId))
                .ReturnsAsync(customer);

            _partnersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(request.PartnerId))
                .ReturnsAsync(partner);

            _preferencesRepositoryMock
                .Setup(repo => repo.GetByIdAsync(request.PreferenceId))
                .ReturnsAsync((Preference)null);

            // Act
            var result = await _controller.GivePromoCodeAsync(request);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = (NotFoundObjectResult)result;
            notFoundResult.Value.Should().Be("Предпочтение не найдено");
        }

        /// <summary>
        /// Должен вернуть BadRequest при отсутствии предпочтения у клиента
        /// </summary>
        [Fact]
        public async Task GivePromoCodeAsync_ShouldReturnBadRequest_WhenCustomerDoesNotHavePreference()
        {
            // Arrange
            var request = _fixture.Create<GivePromoCodeRequest>();
            var customer = CreateCustomerWithoutPreference(request.CustomerId);
            var partner = CreatePartner(request.PartnerId);
            var preference = CreatePreference(request.PreferenceId);

            SetupRepositories(customer, partner, preference);

            // Act
            var result = await _controller.GivePromoCodeAsync(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = (BadRequestObjectResult)result;
            badRequestResult.Value.Should().Be("У клиента нет указанного предпочтения");
        }

        /// <summary>
        /// Должен добавить промокод в базу данных при успешном запросе
        /// </summary>
        [Fact]
        public async Task GivePromoCodeAsync_ShouldAddPromoCodeToRepository_WhenPromoCodeIsGivenSuccessfully()
        {
            // Arrange
            var request = _fixture.Create<GivePromoCodeRequest>();
            var customer = CreateCustomerWithPreference(request.CustomerId, request.PreferenceId);
            var partner = CreatePartner(request.PartnerId);
            var preference = CreatePreference(request.PreferenceId);

            SetupRepositories(customer, partner, preference);

            // Act
            await _controller.GivePromoCodeAsync(request);

            // Assert
            _promoCodesRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<PromoCode>()),
                Times.Once
            );
        }

        #region Helper Methods

        private Customer CreateCustomerWithPreference(Guid customerId, Guid preferenceId)
        {
            var customer = new Customer
            {
                Id = customerId,
                FirstName = "Test",
                LastName = "Customer",
                Email = "test@example.com",
                Preferences = new List<CustomerPreference>
                {
                    new CustomerPreference
                    {
                        CustomerId = customerId,
                        PreferenceId = preferenceId
                    }
                }
            };

            return customer;
        }

        private Customer CreateCustomerWithoutPreference(Guid customerId)
        {
            var customer = new Customer
            {
                Id = customerId,
                FirstName = "Test",
                LastName = "Customer",
                Email = "test@example.com",
                Preferences = new List<CustomerPreference>()
            };

            return customer;
        }

        private Partner CreatePartner(Guid partnerId)
        {
            var partner = new Partner
            {
                Id = partnerId,
                Name = "Test Partner",
                NumberIssuedPromoCodes = 0,
                IsActive = true,
                PartnerManager = new Employee
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com"
                },
                PartnerLimits = new List<PartnerPromoCodeLimit>()
            };

            return partner;
        }

        private Preference CreatePreference(Guid preferenceId)
        {
            var preference = new Preference
            {
                Id = preferenceId,
                Name = "Test Preference"
            };

            return preference;
        }

        private void SetupRepositories(Customer customer, Partner partner, Preference preference)
        {
            _customersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(customer.Id))
                .ReturnsAsync(customer);

            _partnersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(partner.Id))
                .ReturnsAsync(partner);

            _preferencesRepositoryMock
                .Setup(repo => repo.GetByIdAsync(preference.Id))
                .ReturnsAsync(preference);
        }

        #endregion
    }
}