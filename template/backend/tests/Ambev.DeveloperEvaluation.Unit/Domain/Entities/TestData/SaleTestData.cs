using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class SaleTestData
{
    public static CustomerInfo CreateDefaultCustomer()
    {
        return new CustomerInfo(
            Guid.NewGuid(),
            "John Doe",
            "john@example.com",
            "1199999-9999",
            UserRole.Admin
        );
    }

    public static Sale CreateDefaultSale()
    {
        return Sale.Create(
            CreateDefaultCustomer(),
            Guid.NewGuid(),
            "Main Store"
        );
    }

    public static Sale CreateSaleWithItem(
        int quantity = 1,
        decimal unitPrice = 100m,
        string productName = "ProductMigration 1",
        string productCode = "PROD-001")
    {
        var sale = CreateDefaultSale();
        // sale.AddItem(Guid.NewGuid(), productName, productCode, quantity, unitPrice);
        return sale;
    }

    public static class Products
    {
        public static (Guid Id, string Name, string Code) Default => 
            (Guid.NewGuid(), "ProductMigration 1", "PROD-001");
    }

    public static class Items
    {
        public static SaleProduct CreateDefault(
            int quantity = 1,
            Guid? productId = null)
        {
            return new SaleProduct { ProductId = productId ?? Guid.NewGuid(), Quantity = quantity };
        }

        public static readonly decimal DefaultUnitPrice = 100m;
        public static readonly string DefaultProductName = "Test ProductMigration";
        public static readonly string DefaultProductCode = "PROD-001";
    }
}
