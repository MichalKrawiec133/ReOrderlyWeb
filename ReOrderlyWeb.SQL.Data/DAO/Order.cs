using System.ComponentModel.DataAnnotations;

namespace ReOrderlyWeb.SQL.Data.DAO;

public class Order
{
    [Key]
    public int orderId { get; set; }
    [Required]
    public int idUser { get; set; }
    [Required]
    public int orderStatus { get; set; }
    [Required]
    public DateTime orderDate { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<OrderStatus> OrderStatuss { get; set; } = new List<OrderStatus>();

}