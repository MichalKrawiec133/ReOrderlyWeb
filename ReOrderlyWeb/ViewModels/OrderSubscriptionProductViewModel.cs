namespace ReOrderlyWeb.ViewModels;

public class OrderSubscriptionProductViewModel
{
    public int orderSubscriptionProductId { get; set; }
    public int productId { get; set; }
    public string productName { get; set; }
    public double productPrice { get; set; }
    public int productQuantity { get; set; }    
}