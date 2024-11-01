using Microsoft.AspNetCore.Mvc;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.ViewModels;

namespace ReOrderlyWeb.Controllers;
[ApiController]
public class OrderStatusContoller: ControllerBase
{
    private readonly ReOrderlyWebDbContext _context;

    public OrderStatusContoller(ReOrderlyWebDbContext context)
    {
        _context = context;
    }

    [HttpGet("getStatus")]
    public IActionResult GetOrderStatus()
    {
        var orderStatus = _context.OrderStatus.Select(status => new OrderStatusViewModel
        {
            orderStatusId = status.orderStatusId,
            orderStatusDescription = status.orderStatusDescription

        }).ToList();

        return Ok(orderStatus);

    }
    

}