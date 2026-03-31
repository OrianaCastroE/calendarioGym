// using Domain.DTOs.ProductDTOs;
using Domain.DTOs.ProductDTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpPost]
    public IActionResult CreateProduct([FromBody] CreateProductDto newProduct)
    {
        try
        {
            _productService.CreateProduct(newProduct);
            return Created("Product created correctly.", null);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult GetProducts([FromQuery] string? productLine, [FromQuery] List<string>? categories, [FromQuery] string? name)
    {
        var products = _productService.GetProducts(productLine, categories, name);
        return Ok(products);
    }
}
