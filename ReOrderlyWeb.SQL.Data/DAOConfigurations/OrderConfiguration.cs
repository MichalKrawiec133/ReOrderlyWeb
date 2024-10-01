using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.SQL.Data.DAOConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.orderId);
        
        builder.HasOne(o => o.User)
            .WithMany(u => u.Orders)     
            .HasForeignKey(o => o.idUser)
            .IsRequired();
        builder.Property(o => o.idUser).IsRequired();
        //builder.Property(o => o.idOrderStatus).IsRequired();
        //builder.HasOne(o => o.OrderStatus); // czy to dobrze zadziala 
        builder.HasOne(o => o.OrderStatus)
            .WithMany()
            .HasForeignKey(o => o.idOrderStatus)  // Jeśli `Order` posiada `idOrderStatus` jako klucz obcy
            .IsRequired();
        builder.Property(o => o.orderDate).IsRequired();
        builder.ToTable("Order");
    }
}