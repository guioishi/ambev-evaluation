using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateSaleWithCorrectProperties()
    {
        // Arrange
        var customer = SaleTestData.CreateDefaultCustomer();
        var branchId = Guid.NewGuid();
        var branchName = "Main Store";

        // Act
        var sale = Sale.Create(customer, branchId, branchName);

        // Assert
        sale.Should().NotBeNull();
        sale.Id.Should().NotBeEmpty();
        sale.SaleNumber.Should().StartWith("SALE-");
        sale.SaleDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        sale.Customer.Should().Be(customer);
        sale.Branch.BranchId.Should().Be(branchId);
        sale.Branch.BranchName.Should().Be(branchName);
        sale.TotalAmount.Should().Be(0);
        sale.IsCancelled.Should().BeFalse();
        sale.SaleProducts.Should().BeEmpty();
    }

    [Fact]
    public void AddItem_WithNewProduct_ShouldAddItemToSale()
    {
        // Arrange
        var sale = SaleTestData.CreateDefaultSale();
        var (productId, _, _) = SaleTestData.Products.Default;

        // Act
        sale.AddProduct(productId, 2);
        sale.CalculateTotalPrice(100m, 2);

        // Assert
        sale.SaleProducts.Should().HaveCount(1);
        var item = sale.SaleProducts.First();
        item.Quantity.Should().Be(2);
        sale.TotalAmount.Should().Be(200m);
    }

    [Fact]
    public void AddItem_WithExistingProduct_ShouldCombineQuantities()
    {
        // Arrange
        var sale = SaleTestData.CreateDefaultSale();
        var (productId, _, _) = SaleTestData.Products.Default;

        // Act
        sale.AddProduct(productId, 2);
        sale.AddProduct(productId, 3);

        sale.CalculateTotalPrice(100m, 5);

        // Assert
        sale.SaleProducts.Should().HaveCount(1);
        var totalQuantity = sale.SaleProducts.Where(x => x.ProductId == productId).Sum(x => x.Quantity);
        totalQuantity.Should().Be(5);
        sale.TotalAmount.Should().Be(450m);
    }

    [Fact]
    public void AddItem_WithQuantityExceeding20_ShouldThrowDomainException()
    {
        // Arrange
        var sale = SaleTestData.CreateDefaultSale();
        var (productId, _, _) = SaleTestData.Products.Default;

        // Act & Assert
        sale.Invoking(s => s.AddProduct(productId, 21))
            .Should().Throw<DomainException>()
            .WithMessage("Cannot add more than 20 units of the same product");
    }

    [Theory]
    [InlineData(3, 300)]
    [InlineData(4, 360)]
    [InlineData(10, 800)]
    public void AddItem_WithDifferentQuantities_ShouldCalculateCorrectTotal(
        int quantity, decimal expectedTotal)
    {
        // Arrange
        var sale = SaleTestData.CreateDefaultSale();
        var (productId, _, _) = SaleTestData.Products.Default;

        // Act
        sale.AddProduct(productId, quantity);
        sale.CalculateTotalPrice(100m, quantity);

        // Assert
        sale.TotalAmount.Should().Be(expectedTotal);
    }

    [Fact]
    public void Cancel_ShouldMarkSaleAsCancelled()
    {
        // Arrange
        var sale = SaleTestData.CreateDefaultSale();

        // Act
        sale.Cancel();

        // Assert
        sale.IsCancelled.Should().BeTrue();
    }

    [Fact]
    public void Cancel_WhenAlreadyCancelled_ShouldThrowDomainException()
    {
        // Arrange
        var sale = SaleTestData.CreateDefaultSale();
        sale.Cancel();

        // Act & Assert
        sale.Invoking(s => s.Cancel())
            .Should().Throw<DomainException>()
            .WithMessage("Sale is already cancelled");
    }
}
