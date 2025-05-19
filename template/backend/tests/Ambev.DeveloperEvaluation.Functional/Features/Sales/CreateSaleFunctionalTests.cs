using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Domain.Entities.Product;
using Ambev.DeveloperEvaluation.Functional.Utilities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Features.Sales;

public class CreateSaleFunctionalTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public CreateSaleFunctionalTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.Timeout = TimeSpan.FromSeconds(90);
        _client.DefaultRequestHeaders.Add("X-Testing", "true");
    }

    [Fact]
    public async Task CreateSale_WithValidToken_ShouldReturn201()
    {
        // Arrange
        var product = Product.Create(
            title: "Brahma Beer 600ml",
            price: 5.90m,
            description: "Lager Beer",
            category: "Beverages",
            imageUrl: "https://example.com/brahma.jpg",
            rate: 4.5,
            count: 200
        );

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DefaultContext>();
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
        }

        var request = new
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            BranchName = "Store",
            Items = new[]
            {
                new
                {
                    ProductId = product.Id,
                    Quantity = 2
                }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/sales", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
