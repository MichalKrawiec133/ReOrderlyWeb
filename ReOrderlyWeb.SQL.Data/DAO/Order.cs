using System.ComponentModel.DataAnnotations;

namespace ReOrderlyWeb.SQL.Data.DAO;

public class Order
{
    [Key]
    public int orderId { get; set; }
    [Required]
    public int idUser { get; set; }
    public virtual User User { get; set; }
    
    [Required]
    public int idOrderStatus { get; set; }
    
    public virtual OrderStatus OrderStatus { get; set; }
    
    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
    [Required]
    public DateTime orderDate { get; set; }
    
   

}