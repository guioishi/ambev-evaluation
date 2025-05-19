namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.GetProducts;

public class GetProductsRequest
{
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetProductsRequest(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
