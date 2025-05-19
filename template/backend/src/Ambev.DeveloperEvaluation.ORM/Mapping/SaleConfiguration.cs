using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales", "public"); // Explicit schema

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasColumnName("Id")
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        // Scalar properties
        builder.Property(s => s.SaleNumber)
            .HasColumnName("SaleNumber")
            .HasColumnType("character varying(50)")
            .IsRequired();

        builder.Property(s => s.SaleDate)
            .HasColumnName("SaleDate")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(s => s.TotalAmount)
            .HasColumnName("TotalAmount")
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(s => s.IsCancelled)
            .HasColumnName("IsCancelled")
            .HasColumnType("boolean")
            .HasDefaultValue(false);

        builder.Property(s => s.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW()");

        builder.Property(s => s.UpdatedAt)
            .HasColumnName("UpdatedAt")
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);

        // Owned types with explicit type mapping
        builder.OwnsOne(s => s.Customer, c =>
        {
            c.Property(x => x.CustomerId)
                .HasColumnName("CustomerId")
                .HasColumnType("uuid")
                .IsRequired();

            c.Property(x => x.UserName)
                .HasColumnName("CustomerUserName")
                .HasColumnType("character varying(100)")
                .IsRequired();

            c.Property(x => x.Email)
                .HasColumnName("CustomerEmail")
                .HasColumnType("character varying(150)");

            c.Property(x => x.Phone)
                .HasColumnName("CustomerPhone")
                .HasColumnType("character varying(20)");

            c.Property(x => x.Category)
                .HasColumnName("CustomerCategory")
                .HasColumnType("character varying(50)");
        });

        builder.OwnsOne(s => s.Branch, b =>
        {
            b.Property(x => x.BranchId)
                .HasColumnName("BranchId")
                .HasColumnType("uuid")
                .IsRequired();

            b.Property(x => x.BranchName)
                .HasColumnName("BranchName")
                .HasColumnType("character varying(100)")
                .IsRequired();
        });

        // Navigation property (explicitly ignored in queries)
        builder.HasMany(s => s.SaleProducts)
            .WithOne(sp => sp.Sale)
            .HasForeignKey(sp => sp.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SaleProductConfiguration : IEntityTypeConfiguration<SaleProduct>
{
    public void Configure(EntityTypeBuilder<SaleProduct> builder)
    {
        builder.ToTable("SaleProducts", "public"); // Explicit schema

        builder.HasKey(sp => new { sp.SaleId, sp.ProductId });

        builder.Property(sp => sp.SaleId)
            .HasColumnName("SaleId")
            .HasColumnType("uuid");

        builder.Property(sp => sp.ProductId)
            .HasColumnName("ProductId")
            .HasColumnType("uuid");

        builder.Property(sp => sp.Quantity)
            .HasColumnName("Quantity")
            .HasColumnType("integer")
            .IsRequired();

        // Relationships
        builder.HasOne(sp => sp.Sale)
            .WithMany(s => s.SaleProducts)
            .HasForeignKey(sp => sp.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sp => sp.Product)
            .WithMany()
            .HasForeignKey(sp => sp.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
