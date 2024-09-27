using System.ComponentModel.DataAnnotations;

namespace ReOrderlyWeb.SQL.Data.DAO;

public class Order
{
    [Key]
    public int orderId { get; set; }
    [Required]
    public int idUser { get; set; }
    // public virtual User User { get; set; }
    
    [Required]
    public int idOrderStatus { get; set; }
    
   // public OrderStatus OrderStatus { get; set; }
    [Required]
    public DateTime orderDate { get; set; }
    
    //cos jest z kluczami nie tak... oraz nie dodają się pierwsze dwa ordery w bazie

}