using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.SQL.Data.DAOConfigurations;

public class OrderSubscriptionProductConfiguration : IEntityTypeConfiguration<OrderSubscriptionProduct>
{
    public void Configure(EntityTypeBuilder<OrderSubscriptionProduct> builder)
    {
     
        builder.HasKey(osp => osp.orderSubscriptionProductId);
        builder.HasOne(osp => osp.OrderSubscription)
            .WithMany(os => os.orderSubscriptionProducts)
            .HasForeignKey(osp => osp.orderSubscriptionId)
            .IsRequired();
        builder.HasOne(osp => osp.Product)
            .WithMany(p => p.OrderSubscriptionProducts)
            .HasForeignKey(osp => osp.productId) 
            .IsRequired();
        builder.Property(osp => osp.productQuantity).IsRequired();
        builder.ToTable("OrderSubscriptionProduct");
    }
}
