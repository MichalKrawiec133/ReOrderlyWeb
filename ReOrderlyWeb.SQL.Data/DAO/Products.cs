using System.ComponentModel.DataAnnotations;

namespace ReOrderlyWeb.SQL.Data.DAO;

public class Products
{
    [Key]
    public int productId { get; set; }
    [Required]
    [MaxLength(32)]
    public string productName { get; set; }
    [Required]
    public double productPrice { get; set; }
    [Required]
    public int productQuantity { get; set; }

}