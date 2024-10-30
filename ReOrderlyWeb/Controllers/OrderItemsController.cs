using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.SQL.Data.DAO;
using ReOrderlyWeb.ViewModels;

namespace ReOrderlyWeb.Controllers;

[Authorize]
[ApiController]
public class OrderItemsController : ControllerBase
{
    private readonly ReOrderlyWebDbContext _context;

    public OrderItemsController(ReOrderlyWebDbContext context)
    {
        _context = context;
    }

    // pobierz itemy dla orderitems
    [HttpGet("{orderId}")]
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

        // Sprawdzenie, czy zamówienie należy do użytkownika
        var order = _context.Order.SingleOrDefault(o => o.orderId == orderId && o.idUser == user.userId);
        if (order == null)
        {
            return NotFound("Order not found for the given user.");
        }

        
        var orderItems = _context.OrderItems
            .Where(oi => oi.idOrder == orderId)
            .Select(oi => new OrderItemsViewModel
            {
                orderItemId = oi.orderItemId,
                idProduct = oi.idProduct,
                Products = new ProductsViewModel
                {
                    productId = oi.Products.productId,
                    productName = oi.Products.productName,
                    productPrice = oi.Products.productPrice,
                    productQuantity = oi.Products.productQuantity,
                    imagePath = oi.Products.imagePath,
                },
                idOrder = oi.idOrder,
                orderItemQuantity = oi.orderItemQuantity,
                orderPrice = oi.orderPrice
            })
            .ToList();

        if (orderItems == null || !orderItems.Any())
        {
            return NotFound("No order items found for this order.");
        }

        return Ok(orderItems);
    }

    // dodaj item do ordera
    [HttpPost("addItem")] 
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
            orderPrice = orderItemViewModel.orderPrice,
        };

        _context.OrderItems.Add(newOrderItem);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Order item added successfully." });
    }

    // aktualizacja itemu w orderitems
    [HttpPatch("{orderItemId}")]
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

    // usuniecie itemorder
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
