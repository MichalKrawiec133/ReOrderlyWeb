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
            .WithMany()    
            .HasForeignKey(oi => oi.idOrder)
            .IsRequired();

        builder.HasOne(oi => oi.Products)
            .WithMany()                     
            .HasForeignKey(oi => oi.idProduct)
            .IsRequired();

        
        builder.Property(oi => oi.orderItemQuantity).IsRequired();
        builder.Property(oi => oi.orderPrice).IsRequired();

        builder.ToTable("OrderItems");
    }
}