// using Ambev.DeveloperEvaluation.Domain.Enums;
// using Ambev.DeveloperEvaluation.Domain.Exceptions;
// using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
// using FluentAssertions;
// using Xunit;
//
// namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;
//
// public class SaleItemTests
// {
//     [Fact]
//     public void Constructor_WithValidData_ShouldCreateSaleItem()
//     {
//         // Arrange
//         var productId = Guid.NewGuid();
//         var saleId = Guid.NewGuid();
//
//         // Act
//         var item = SaleTestData.Items.CreateDefault(
//             quantity: 2,
//             productId: productId,
//             saleId: saleId);
//
//         // Assert
//         item.Should().NotBeNull();
//         item.Quantity.Should().Be(2);
//         item.SaleId.Should().Be(saleId);
//     }
//
//     [Theory]
//     [InlineData(0, "Quantity must be greater than zero")]
//     [InlineData(-1, "Quantity must be greater than zero")]
//     [InlineData(21, "Cannot sell more than 20 identical items")]
//     public void Constructor_WithInvalidQuantity_ShouldThrowDomainException(int invalidQuantity, string expectedMessage)
//     {
//         // Act & Assert
//         var act = () => SaleTestData.Items.CreateDefault(quantity: invalidQuantity);
//
//         act.Should().Throw<DomainException>()
//             .WithMessage(expectedMessage);
//     }
//
//     [Fact]
//     public void SetQuantity_WithOneItem_ShouldHaveNoDiscount()
//     {
//         var item = SaleTestData.Items.CreateDefault(quantity: 1);
//         item.DiscountTier.Should().Be(DiscountTier.None);
//     }
//
//     [Fact]
//     public void SetQuantity_WithThreeItems_ShouldHaveNoDiscount()
//     {
//         var item = SaleTestData.Items.CreateDefault(quantity: 3);
//         item.DiscountTier.Should().Be(DiscountTier.None);
//     }
//
//     [Fact]
//     public void SetQuantity_WithFourItems_ShouldHaveTenPercentDiscount()
//     {
//         var item = SaleTestData.Items.CreateDefault(quantity: 4);
//         item.DiscountTier.Should().Be(DiscountTier.TenPercent);
//     }
//
//     [Fact]
//     public void SetQuantity_WithNineItems_ShouldHaveTenPercentDiscount()
//     {
//         var item = SaleTestData.Items.CreateDefault(quantity: 9);
//         item.DiscountTier.Should().Be(DiscountTier.TenPercent);
//     }
//
//     [Fact]
//     public void SetQuantity_WithTenItems_ShouldHaveTwentyPercentDiscount()
//     {
//         var item = SaleTestData.Items.CreateDefault(quantity: 10);
//         item.DiscountTier.Should().Be(DiscountTier.TwentyPercent);
//     }
//
//     [Fact]
//     public void SetQuantity_WithTwentyItems_ShouldHaveTwentyPercentDiscount()
//     {
//         var item = SaleTestData.Items.CreateDefault(quantity: 20);
//         item.DiscountTier.Should().Be(DiscountTier.TwentyPercent);
//     }
//
//     [Theory]
//     [InlineData(1, 100, 0)]
//     [InlineData(3, 100, 0)]
//     [InlineData(4, 100, 40)]
//     [InlineData(9, 100, 90)]
//     [InlineData(10, 100, 200)]
//     [InlineData(20, 100, 400)]
//     public void Discount_ShouldCalculateCorrectValue(int quantity, int unitPrice, int expectedDiscount)
//     {
//         // Arrange
//         var item = SaleTestData.Items.CreateDefault(quantity, unitPrice);
//
//         // Act & Assert
//         item.Discount.Should().Be(expectedDiscount);
//     }
//
//     [Theory]
//     [InlineData(1, 100, 100)]
//     [InlineData(3, 100, 300)]
//     [InlineData(4, 100, 360)]
//     [InlineData(9, 100, 810)]
//     [InlineData(10, 100, 800)]
//     [InlineData(20, 100, 1600)]
//     public void TotalPrice_ShouldCalculateCorrectValue(int quantity, int unitPrice, int expectedTotal)
//     {
//         // Arrange
//         var item = SaleTestData.Items.CreateDefault(quantity, unitPrice);
//
//         // Act & Assert
//         item.TotalPrice.Should().Be(expectedTotal);
//     }
//
//     [Fact]
//     public void SetQuantity_WithExistingQuantityAbove20_ShouldThrowDomainException()
//     {
//         // Arrange
//         var item = SaleTestData.Items.CreateDefault(quantity: 5);
//
//         // Act & Assert
//         item.Invoking(i => i.SetQuantity(21))
//             .Should().Throw<DomainException>()
//             .WithMessage("Cannot sell more than 20 identical items");
//     }
// }
