using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Product;

public class AddProductsToSaleCommand : IRequest<SaleResultDto>
{
    public Guid SaleId { get; set; }
    public List<ProductSaleRequest> Items { get; set; } = [];

    public class ProductSaleRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
