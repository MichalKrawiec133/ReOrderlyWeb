using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.SQL.Data.DAOConfigurations;

public class OrderSubscriptionProductConfiguration : IEntityTypeConfiguration<OrderSubscriptionProduct>
{
    public void Configure(EntityTypeBuilder<OrderSubscriptionProduct> builder)
    {
        // Primary key
        builder.HasKey(osp => osp.orderSubscriptionProductId);
        
        // Foreign key relationship with OrderSubscription
        builder.HasOne(osp => osp.OrderSubscription)
            .WithMany(os => os.OrderSubscriptionProducts)
            .HasForeignKey(osp => osp.orderSubscriptionId)
            .IsRequired();

        // Foreign key relationship with Product
        builder.HasOne(osp => osp.Product)
            .WithMany(p => p.OrderSubscriptionProducts)
            .HasForeignKey(osp => osp.productId) // Assuming `productId` is the foreign key
            .IsRequired();

        // Other required properties
        builder.Property(osp => osp.productQuantity).IsRequired();

        // Mapping to the table
        builder.ToTable("OrderSubscriptionProduct");
    }
}
