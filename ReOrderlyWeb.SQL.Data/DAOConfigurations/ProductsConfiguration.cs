using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.SQL.Data.DAOConfigurations;

public class ProductsConfiguration : IEntityTypeConfiguration<Products>
{
    public void Configure(EntityTypeBuilder<Products> builder)
    {
        builder.HasKey(p => p.productId);

        builder.Property(p => p.productName).IsRequired().HasMaxLength(32);
        builder.Property(p => p.productPrice).IsRequired();
        builder.Property(p => p.productQuantity).IsRequired();
        builder.HasMany(p => p.OrderSubscriptionProducts)
            .WithOne(osp => osp.Product)
            .HasForeignKey(osp => osp.productId)
            .IsRequired();
        builder.Property(p => p.imagePath);
        builder.ToTable("Product");
    }
}
