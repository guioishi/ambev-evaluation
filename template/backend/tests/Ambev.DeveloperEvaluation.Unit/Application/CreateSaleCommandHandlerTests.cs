using Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Services.Interface;
using Ambev.DeveloperEvaluation.Common.Interface;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.Product;
using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events.SaleEvents;
using Ambev.DeveloperEvaluation.Domain.Exceptions.Product;
using Ambev.DeveloperEvaluation.Domain.Exceptions.Sale;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.LogExtensionHelper;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateSaleCommandHandlerTests
{
    private readonly Mock<ISaleRepository> _repositoryMock;
    private readonly Mock<IEventPublisher> _eventPublisherMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<CreateSaleCommandHandler>> _loggerMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly CreateSaleCommandHandler _handler;

    public CreateSaleCommandHandlerTests()
    {
        _repositoryMock = new Mock<ISaleRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _cacheServiceMock = new Mock<ICacheService>();
        _eventPublisherMock = new Mock<IEventPublisher>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<CreateSaleCommandHandler>>();

        _handler = new CreateSaleCommandHandler(
            _repositoryMock.Object,
            _mapperMock.Object,
            _eventPublisherMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object,
            _productRepositoryMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateSaleAndPublishEvent()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var saleResultDto = CreateSaleHandlerTestData.GenerateSaleResultDto();
        var customerInfo = CreateSaleHandlerTestData.GenerateCustomer();
        var product = CreateSaleHandlerTestData.GenerateValidProduct();

        _mapperMock.Setup(m => m.Map<CustomerInfo>(It.IsAny<CustomerInfoDto>())).Returns(customerInfo);
        _mapperMock.Setup(m => m.Map<SaleResultDto>(It.IsAny<Sale>())).Returns(saleResultDto);

        _cacheServiceMock
            .Setup(c => c.GetAsync<Product>(It.IsAny<string>()))
            .ReturnsAsync((Product?)null);

        _productRepositoryMock
            .Setup(p => p.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _cacheServiceMock
            .Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()))
            .Returns(Task.FromResult(true));

        _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(saleResultDto.SaleNumber, result.SaleNumber);

        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Once);
        _eventPublisherMock.Verify(e => e.PublishAsync(RedisChannels.SaleEvents, It.IsAny<SaleCreatedEvent>()),
            Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_EventPublishingFails_ShouldThrowSaleEventPublishingFailedException()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var product = CreateSaleHandlerTestData.GenerateValidProduct();
        var customerInfo = CreateSaleHandlerTestData.GenerateCustomer();

        _mapperMock.Setup(m => m.Map<CustomerInfo>(It.IsAny<CustomerInfoDto>())).Returns(customerInfo);
        _mapperMock.Setup(m => m.Map<SaleResultDto>(It.IsAny<Sale>()))
            .Returns(CreateSaleHandlerTestData.GenerateSaleResultDto());

        _cacheServiceMock
            .Setup(c => c.GetAsync<Product>($"product:{product.Id}"))
            .ReturnsAsync((Product?)null);

        _productRepositoryMock
            .Setup(p => p.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _eventPublisherMock
            .Setup(e => e.PublishAsync(RedisChannels.SaleEvents, It.IsAny<SaleCreatedEvent>()))
            .ThrowsAsync(new Exception("Failure"));

        _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);

        // Act & Assert
        await Assert.ThrowsAsync<SaleEventPublishingFailedException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_RepositoryFails_ShouldRollbackAndThrowSaleCreationFailedException()
    {
        // Arrange
        var branchId = Guid.NewGuid();
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var exception = new Exception("Failure");
        var product = CreateSaleHandlerTestData.GenerateValidProduct();

        _cacheServiceMock
            .Setup(c => c.GetAsync<Product>($"product:{product.Id}"))
            .ReturnsAsync((Product?)null);

        _productRepositoryMock
            .Setup(p => p.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _repositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act & Assert
        await Assert.ThrowsAsync<SaleCreationFailedException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);

        _loggerMock.VerifyLog(LogLevel.Error, exception);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString()!.Contains("Error creating sale for branch") &&
                    v.ToString()!.Contains("Rolling back transaction")),
                It.Is<Exception>(ex => ex is Exception),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ShouldThrowProductNotFoundException()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var productId = command.Items[0].ProductId;
        var customerInfo = CreateSaleHandlerTestData.GenerateCustomer();

        _mapperMock
            .Setup(m => m.Map<CustomerInfo>(It.IsAny<CustomerInfoDto>()))
            .Returns(customerInfo);

        _cacheServiceMock
            .Setup(c => c.GetAsync<Product>($"product:{productId}"))
            .ReturnsAsync((Product?)null);

        _productRepositoryMock
            .Setup(p => p.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ProductException.ProductNotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ProductFoundInCache_ShouldSkipRepositoryCall()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var product = CreateSaleHandlerTestData.GenerateValidProduct();
        var customerInfo = CreateSaleHandlerTestData.GenerateCustomer();

        _mapperMock
            .Setup(m => m.Map<CustomerInfo>(It.IsAny<CustomerInfoDto>()))
            .Returns(customerInfo);

        _mapperMock
            .Setup(m => m.Map<SaleResultDto>(It.IsAny<Sale>()))
            .Returns(CreateSaleHandlerTestData.GenerateSaleResultDto());

        _cacheServiceMock
            .Setup(c => c.GetAsync<Product>(It.IsAny<string>()))
            .ReturnsAsync(product);

        _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _productRepositoryMock.Verify(p =>
                p.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);

        Assert.NotNull(result);
    }
}
