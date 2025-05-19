using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;
using Ambev.DeveloperEvaluation.Domain.Entities.Product;
using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for sales-related tests using Bogus
/// </summary>
public static class CreateSaleHandlerTestData
{
    private static readonly Faker<SaleProductDto> SaleItemFaker = new Faker<SaleProductDto>()
        .CustomInstantiator(f => new SaleProductDto(
            ProductId: Guid.NewGuid(),
            Quantity: f.Random.Int(1, 10)
        ));

    private static readonly Faker<CustomerInfo> CustomerInfoFaker = new Faker<CustomerInfo>()
        .CustomInstantiator(f => new CustomerInfo(
            CustomerId: Guid.NewGuid(),
            UserName: f.Internet.UserName(),
            Email: f.Internet.Email(),
            Phone: f.Phone.PhoneNumber("15151515"),
            Category: f.PickRandom(UserRole.Admin, UserRole.Customer, UserRole.Manager, UserRole.None)
        ));

    private static readonly Faker<CustomerInfoDto> CustomerFaker = new Faker<CustomerInfoDto>()
        .CustomInstantiator(f => new CustomerInfoDto(
            CustomerId: Guid.NewGuid(),
            UserName: f.Internet.UserName(),
            Email: f.Internet.Email(),
            Phone: f.Phone.PhoneNumber("15151515"),
            Category: f.PickRandom("Admin", "Customer", "Manager")
        ));

    private static readonly Faker<SaleResultDto> SaleResultDtoFaker = new Faker<SaleResultDto>()
        .CustomInstantiator(f => new SaleResultDto(
            SaleNumber: $"SALE-{f.Random.Int(1000, 9999)}",
            SaleDate: f.Date.Recent(),
            Customer: CustomerFaker.Generate(),
            Branch: new BranchInfoDto(Guid.NewGuid(), f.Company.CompanyName()),
            TotalAmount: f.Random.Decimal(100, 10000),
            IsCancelled: false,
            Items: GenerateSaleItemResults(f.Random.Int(1, 5))
        ));

    private static List<GetSaleProductDto> GenerateSaleItemResults(int count)
    {
        var faker = new Faker();
        var items = new List<GetSaleProductDto>();

        for (var i = 0; i < count; i++)
        {
            items.Add(new GetSaleProductDto(
                ProductId: Guid.NewGuid(),
                Quantity: faker.Random.Int(1, 10)));
        }

        return items;
    }

    private static readonly Faker<Product> ProductFaker = new Faker<Product>()
        .CustomInstantiator(f => Product.Create(
            title: f.Commerce.ProductName(),
            price: f.Random.Decimal(10, 500),
            description: f.Commerce.ProductDescription(),
            category: f.Commerce.Categories(1).First(),
            imageUrl: f.Image.PicsumUrl(),
            rate: f.Random.Double(1, 5),
            count: f.Random.Int(1, 500)
        ));

    /// <summary>
    /// Generates a valid CreateSaleCommand with specific number of items
    /// </summary>
    public static CreateSaleCommand GenerateValidCommand(int itemCount = 1)
    {
        var command = new CreateSaleCommand(
            BranchId: Guid.NewGuid(),
            BranchName: new Faker().Company.CompanyName(),
            Items: SaleItemFaker.Generate(itemCount)
        )
        {
            Customer = CustomerFaker.Generate()
        };

        return command;
    }

    /// <summary>
    /// Generates a SaleResultDto with random data
    /// </summary>
    public static SaleResultDto GenerateSaleResultDto()
    {
        return SaleResultDtoFaker.Generate();
    }

    /// <summary>
    /// Generates a SaleResultDto with random data
    /// </summary>
    public static CustomerInfo GenerateCustomer()
    {
        return CustomerInfoFaker.Generate();
    }

    /// <summary>
    /// Generates a Product with random data
    /// </summary>
    public static Product GenerateValidProduct()
    {
        return ProductFaker.Generate();
    }

    /// <summary>
    /// Generates an invalid CreateSaleCommand (empty items)
    /// </summary>
    public static CreateSaleCommand GenerateInvalidCommand()
    {
        return new CreateSaleCommand(
            BranchId: Guid.Empty,
            BranchName: string.Empty,
            Items: []
        )
        {
            Customer = null!
        };
    }
}
