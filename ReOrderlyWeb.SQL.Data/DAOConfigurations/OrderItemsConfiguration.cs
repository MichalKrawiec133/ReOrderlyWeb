using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.SQL.Data.DAOConfigurations;
public class OrderItemsConfiguration : IEntityTypeConfiguration<OrderItems>
{
    public void Configure(EntityTypeBuilder<OrderItems> builder)
    {
        builder.HasKey(oi => oi.orderItemId);
// TODO COS TU DALEJ JEST Z TYMI KLUCZAMII NIBY DOBRA KONFIGURACJA A JEDNAK WYRZUCA BLAD KLUCZY OBCYCH.
// TODO chyba jednak kolejnosc seed jest zle, moznaby zmienic tak ze orderitems nie ma w sobie orderid, tylko orderid ma w sobie orderitemsid
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.idOrder)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(oi => oi.Product)
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
