namespace ReOrderlyWeb.ViewModels;

public class OrderSubscriptionProductViewModel
{
    public int orderSubscriptionProductId { get; set; }
        
    public ProductsViewModel Products { get; set; }
        
    public int productQuantity { get; set; }    
}