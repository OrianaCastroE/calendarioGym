using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.ProductDTOs;

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
        try
        {
            var products = _productService.GetProducts(productLine, categories, name);
            return Ok(products);
        }
        catch
        {
            return NotFound("Products not found.");
        }
    }

    [HttpGet("most-requested")]
    public IActionResult GetMostRequestedProducts()
    {
        try
        {
            var products = _productService.GetMostRequestedProducts();
            return Ok(products);
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut]
    public IActionResult UpdateProduct([FromBody] ProductDto product)
    {
        try
        {
            _productService.UpdateProduct(product);
            return Ok("Product updated correctly.");
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
