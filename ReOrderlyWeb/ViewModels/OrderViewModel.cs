namespace ReOrderlyWeb.ViewModels;

public class OrderViewModel
{
    public int orderId { get; set; }
    public int idUser { get; set; }
    public int idOrderStatus { get; set; }
    public virtual OrderStatusViewModel OrderStatus { get; set; }
    public DateTime orderDate { get; set; }
    public List<OrderItemsViewModel> orderItems { get; set; }
}
