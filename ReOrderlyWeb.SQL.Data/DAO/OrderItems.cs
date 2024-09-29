using System.ComponentModel.DataAnnotations;

namespace ReOrderlyWeb.SQL.Data.DAO;

public class OrderItems
{
    [Key]
    public int orderItemId { get; set; }
    [Required]
    public int idProduct { get; set; }
    //public virtual Products Products { get; set; }  
    public virtual Products Product { get; set; }
    [Required]
    public int idOrder { get; set; }
    public virtual Order Order { get; set; }  
    [Required]
    public int orderItemQuantity { get; set; }
    [Required]
    public int orderPrice { get; set; }
    
    
}