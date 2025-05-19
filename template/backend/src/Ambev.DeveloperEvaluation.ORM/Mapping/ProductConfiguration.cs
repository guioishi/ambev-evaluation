using Ambev.DeveloperEvaluation.Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(p => p.Id);

        // BaseEntity properties
        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(p => p.UpdatedAt)
            .IsRequired(false);

        // ProductMigration properties
        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(255);

        // Money property
        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2)
            .IsRequired();

        // Value Object - Rating
        builder.OwnsOne(p => p.Rating, r =>
        {
            r.Property(rating => rating.Rate).HasColumnName("RatingRate");
            r.Property(rating => rating.Count).HasColumnName("RatingCount");
        });

        // Indexes
        builder.HasIndex(p => p.Category);
        builder.HasIndex(p => p.Title);
    }
}
