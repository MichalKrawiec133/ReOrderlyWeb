using Microsoft.AspNetCore.Mvc;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.ViewModels;

namespace ReOrderlyWeb.Controllers;

public class ProductController : ControllerBase
{
    private readonly ReOrderlyWebDbContext _context;

    public ProductController(ReOrderlyWebDbContext context)
    {
        _context = context;
    }

    [HttpGet("products")]
    public IActionResult GetProducts()
    {
        var products = _context.Products
            .Select(p => new ProductsViewModel
            {
                productId = p.productId,
                productName = p.productName,
                productPrice = p.productPrice,
                productQuantity = p.productQuantity,
                imagePath = p.imagePath,
            })
            .ToList();

        return Ok(products);
    }

    
    
    
}