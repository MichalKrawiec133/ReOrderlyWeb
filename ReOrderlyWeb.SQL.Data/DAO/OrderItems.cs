using System.ComponentModel.DataAnnotations;

namespace ReOrderlyWeb.SQL.Data.DAO;

public class OrderItems
{
    [Key]
    public int orderItemId { get; set; }
    [Required]
    public int idProduct { get; set; }
    [Required]
    public int idOrder { get; set; }
    [Required]
    public int orderItemQuantity { get; set; }
    [Required]
    public int orderPrice { get; set; }
    
    public virtual ICollection<Products> Productss { get; set; } = new List<Products>();
    public virtual ICollection<Order> Orders  { get; set; } = new List<Order>();
}