using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.SQL.Data.DAOConfigurations;
public class OrderItemsConfiguration : IEntityTypeConfiguration<OrderItems>
{
    public void Configure(EntityTypeBuilder<OrderItems> builder)
    {
        builder.HasKey(oi => oi.orderItemId);
        
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.idOrder)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(oi => oi.Products)
            .WithMany(p => p.OrderItems)  
            .HasForeignKey(oi => oi.idProduct)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.Property(oi => oi.idProduct).IsRequired();
        builder.Property(oi => oi.idOrder).IsRequired();
        builder.Property(oi => oi.orderItemQuantity).IsRequired();
        builder.Property(oi => oi.orderPrice).IsRequired();

        builder.ToTable("OrderItems");
    }
}
