using Ambev.DeveloperEvaluation.Common.Interface;
using Ambev.DeveloperEvaluation.Domain.Entities.Product;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _context;
    private readonly ICacheService _cacheService;
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);
    private const string ProductsCountCacheKey = "products:total-count";

    public ProductRepository(
        DefaultContext context,
        ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await CacheProductAsync(product);
        await InvalidateProductsCountCacheAsync();

        return product;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = GetProductCacheKey(id);
        var cachedProduct = await _cacheService.GetAsync<Product>(cacheKey);
        if (cachedProduct != null)
        {
            return cachedProduct;
        }

        var product = await _context.Products
            .Include(p => p.Rating)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (product != null)
        {
            await CacheProductAsync(product);
        }

        return product;
    }

    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);

        await CacheProductAsync(product);
        await InvalidateProductsCountCacheAsync();

        return product;
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category,
        CancellationToken cancellationToken = default)
    {
        var normalizedCategory = category.Trim();
        return await _context.Products
            .Where(p => p.Category == normalizedCategory)
            .Include(p => p.Rating)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> SearchAsync(
        string? searchTerm = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? category = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim();
            query = query.Where(p =>
                p.Title.Contains(term) ||
                p.Description.Contains(term));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => p.Category == category.Trim());
        }

        return await query
            .Include(p => p.Rating)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
    {
        var cachedCount = await _cacheService.GetAsync<int?>(ProductsCountCacheKey);
        if (cachedCount.HasValue)
        {
            return cachedCount.Value;
        }

        var count = await _context.Products.CountAsync(cancellationToken);
        await _cacheService.SetAsync(ProductsCountCacheKey, count, _cacheExpiration);

        return count;
    }

    public async Task<IEnumerable<Product>> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .OrderBy(p => p.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(p => p.Rating)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    private async Task CacheProductAsync(Product product)
    {
        var cacheKey = GetProductCacheKey(product.Id);
        await _cacheService.SetAsync(cacheKey, product, _cacheExpiration);
    }

    private async Task InvalidateProductsCountCacheAsync()
    {
        await _cacheService.RemoveAsync(ProductsCountCacheKey);
    }

    private static string GetProductCacheKey(Guid productId)
    {
        return $"product:{productId}";
    }
}
