using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.ViewModels;

public class OrderItemsViewModel
{
    public int orderItemId { get; set; }

    public int idProduct { get; set; }
    public virtual ProductsViewModel Products { get; set; }  
    

    public int idOrder { get; set; }
    //public virtual Order Order { get; set; }  

    public int orderItemQuantity { get; set; }
    public int orderPrice { get; set; }
}