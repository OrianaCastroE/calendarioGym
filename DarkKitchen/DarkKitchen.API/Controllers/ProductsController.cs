using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.ProductDTOs;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[Route("api/products")]
[ApiController]

public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpPost]
    public IActionResult CreateProduct([FromBody] CreateProductDto newProduct)
    {
        _productService.CreateProduct(newProduct);
        return Created("Product created correctly.", null);
    }

    [HttpPut]
    public IActionResult UpdateProduct([FromBody] ProductDto product)
    {
        _productService.UpdateProduct(product);
        return Ok("Product updated correctly.");
    }

    [HttpGet]
    public IActionResult GetProducts([FromQuery] string? productLine, [FromQuery] List<string>? categories, [FromQuery] string? name)
    {
        var products = _productService.GetProducts(productLine, categories, name);
        return Ok(products);
    }

    [HttpGet("most-requested")]
    public IActionResult GetMostRequestedProducts()
    {
        var products = _productService.GetMostRequestedProducts();
        return Ok(products);
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateStatus(int id, [FromBody] ProductStatusDto status)
    {
        _productService.UpdateStatus(id, status);
        return Ok("Product status updated correctly.");
    }
}
