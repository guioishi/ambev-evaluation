using Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Controllers;

public class SalesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly SalesController _controller;

    public SalesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _controller = new SalesController(_mediatorMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task CreateSale_ValidRequest_ReturnsCreatedResult()
    {
        // Arrange
        var request = new CreateSaleRequest(
            CustomerId: Guid.NewGuid(),
            BranchId: Guid.NewGuid(),
            BranchName: "Store 1",
            Items: [new SaleProductRequest(Guid.NewGuid(), 1)]
        );

        var command = new CreateSaleCommand(
            BranchId: request.BranchId,
            BranchName: request.BranchName,
            Items: request.Items.Select(i => new SaleProductDto(
                i.ProductId,
                i.Quantity
            )).ToList()
        );

        var saleResultDto = new SaleResultDto(
            SaleNumber: "SALE-123",
            SaleDate: DateTime.UtcNow,
            new CustomerInfoDto(Guid.NewGuid(), "username", "email", "151515", "category"),
            new BranchInfoDto(Guid.Empty, "Store 1"),
            TotalAmount: 10.0m,
            IsCancelled: false,
            Items:
            [
                new GetSaleProductDto(Guid.NewGuid(), 10)
            ]
        );

        var expectedResponse = new CreateSaleResponse(
            SaleNumber: "SALE-123",
            SaleDate: saleResultDto.SaleDate,
            BranchName: "Loja 1",
            TotalAmount: 10.0m,
            Items:
            [
                new SaleItemResponse(Guid.NewGuid(), 10)
            ]
        );

        _mapperMock
            .Setup(m => m.Map<CreateSaleCommand>(request))
            .Returns(command);

        _mapperMock
            .Setup(m => m.Map<CreateSaleResponse>(saleResultDto))
            .Returns(expectedResponse);

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(saleResultDto);

        // Act
        var result = await _controller.CreateSale(request, CancellationToken.None);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdAtActionResult.StatusCode);

        var response = createdAtActionResult.Value as SaleResultDto;

        Assert.NotNull(response);

        Assert.Equal("SALE-123", response.SaleNumber);
        Assert.Equal(10.0m, response.TotalAmount);

        _mapperMock.Verify(m => m.Map<CreateSaleCommand>(request), Times.Once);
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateSale_InvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateSaleRequest(
            CustomerId: Guid.Empty,
            BranchId: Guid.Empty,
            BranchName: "",
            Items: []
        );

        var expectedErrors = new List<ValidationFailure>
        {
            new("BranchId", "'Branch Id' must not be empty."),
            new("Items", "'Items' must not be empty.")
        };

        // Act
        var result = await _controller.CreateSale(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);

        var actualErrors = (badRequestResult.Value as IEnumerable<ValidationFailure>)?.ToList();

        Assert.NotNull(actualErrors);
        Assert.NotEmpty(actualErrors);
        Assert.Equal(expectedErrors.Count, actualErrors.Count);

        foreach (var expectedError in expectedErrors)
        {
            var actualError = actualErrors.FirstOrDefault(e =>
                e.PropertyName == expectedError.PropertyName &&
                e.ErrorMessage == expectedError.ErrorMessage);

            Assert.NotNull(actualError);
        }
    }
}
