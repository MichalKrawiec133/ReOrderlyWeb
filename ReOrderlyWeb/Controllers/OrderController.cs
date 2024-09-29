using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.SQL.Data.DAO;
using ReOrderlyWeb.ViewModels;

namespace ReOrderlyWeb.Controllers;
    

public class OrderController : ControllerBase
{
    private readonly ReOrderlyWebDbContext _context;

    public OrderController(ReOrderlyWebDbContext context)
    {
        _context = context;
    }

    // pobranie wszystkich zamowien
    [HttpGet("orders")]
    [Authorize]
    public IActionResult GetOrders()
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized("No valid user session.");
        }

        var user = _context.User.SingleOrDefault(c => c.emailAddress == email);
        if (user == null)
        {
            return Unauthorized("User not found.");
        }

        var orders = _context.Order
            .Where(o => o.idUser == user.userId)
            .Select(o => new OrderViewModel
            {
                orderId = o.orderId,
                idUser = o.idUser,
                idOrderStatus = o.idOrderStatus,
                orderDate = o.orderDate,
                orderItems = _context.OrderItems
                    .Where(oi => oi.idOrder == o.orderId)
                    .Select(oi => new OrderItemsViewModel
                    {
                        orderItemId = oi.orderItemId,
                        idProduct = oi.idProduct,
                        idOrder = oi.idOrder,
                        orderItemQuantity = oi.orderItemQuantity,
                        orderPrice = oi.orderPrice
                    })
                    .ToList()
            })
            .ToList();

        return Ok(orders);
    }


    // dodaj nowe zamowienie
    [HttpPost("checkout")]
    public async Task<IActionResult> CreateOrder([FromBody] OrderViewModel orderViewModel)
    {
        var order = new Order
        {
            idUser = orderViewModel.idUser,
            idOrderStatus = orderViewModel.idOrderStatus,
            orderDate = orderViewModel.orderDate
        };

        _context.Order.Add(order);
        await _context.SaveChangesAsync();

        foreach (var item in orderViewModel.orderItems)
        {
            var orderItem = new OrderItems
            {
                idOrder = order.orderId,
                idProduct = item.idProduct,
                orderItemQuantity = item.orderItemQuantity,
                orderPrice = item.orderPrice
            };
            _context.OrderItems.Add(orderItem);
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = "Order and items created successfully", orderId = order.orderId });
    }

}


    
