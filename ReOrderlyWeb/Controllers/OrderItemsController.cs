using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.SQL.Data.DAO;
using ReOrderlyWeb.ViewModels;

namespace ReOrderlyWeb.Controllers;


public class OrderItemsController : ControllerBase
{
    private readonly ReOrderlyWebDbContext _context;

    public OrderItemsController(ReOrderlyWebDbContext context)
    {
        _context = context;
    }
//XXXXXXXXXXDDDDDDDDDDDDDDDDDDDDDD TODO: ZMIENIC TAK ZEBY WSZYSTKO BYLO NA JWT, CALKOWICIE USUNAC COOKIES.
    // Get all order items for a specific order
    [HttpGet("{orderId}")]
    [Authorize]
    public IActionResult GetOrderItems(int orderId)
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

        var orderItems = _context.OrderItems
            .Where(oi => oi.idOrder == orderId && oi.Order.idUser == user.userId)
            .Select(oi => new OrderItemsViewModel
            {
                orderItemId = oi.orderItemId,
                idProduct = oi.idProduct,
                idOrder = oi.idOrder,
                orderItemQuantity = oi.orderItemQuantity,
                orderPrice = oi.orderPrice
            })
            .ToList();

        return Ok(orderItems);
    }

    // Add an item to an existing order
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddOrderItem([FromBody] OrderItemsViewModel orderItemViewModel)
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

        var order = _context.Order.SingleOrDefault(o => o.orderId == orderItemViewModel.idOrder && o.idUser == user.userId);
        if (order == null)
        {
            return NotFound("Order not found.");
        }

        var newOrderItem = new OrderItems
        {
            idOrder = orderItemViewModel.idOrder,
            idProduct = orderItemViewModel.idProduct,
            orderItemQuantity = orderItemViewModel.orderItemQuantity,
            orderPrice = orderItemViewModel.orderPrice
        };

        _context.OrderItems.Add(newOrderItem);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Order item added successfully." });
    }

    // Update an order item
    [HttpPatch("{orderItemId}")]
    [Authorize]
    public async Task<IActionResult> UpdateOrderItem(int orderItemId, [FromBody] OrderItemsViewModel orderItemViewModel)
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

        var orderItem = _context.OrderItems.SingleOrDefault(oi => oi.orderItemId == orderItemId && oi.Order.idUser == user.userId);
        if (orderItem == null)
        {
            return NotFound("Order item not found.");
        }

        orderItem.idProduct = orderItemViewModel.idProduct;
        orderItem.orderItemQuantity = orderItemViewModel.orderItemQuantity;
        orderItem.orderPrice = orderItemViewModel.orderPrice;

        _context.OrderItems.Update(orderItem);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Order item updated successfully." });
    }

    // Delete an order item
    [HttpDelete("{orderItemId}")]
    [Authorize]
    public async Task<IActionResult> DeleteOrderItem(int orderItemId)
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

        var orderItem = _context.OrderItems.SingleOrDefault(oi => oi.orderItemId == orderItemId && oi.Order.idUser == user.userId);
        if (orderItem == null)
        {
            return NotFound("Order item not found.");
        }

        _context.OrderItems.Remove(orderItem);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Order item deleted successfully." });
    }
}
