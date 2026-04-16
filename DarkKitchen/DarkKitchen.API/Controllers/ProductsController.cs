using DarkKitchen.Domain.Interfaces.Services;
using DarkKitchen.Models.DateDTOs;
using DarkKitchen.Models.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[Route("api/products")]
[ApiController]

public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult CreateProduct([FromBody] CreateProductDto newProduct)
    {
        _productService.CreateProduct(newProduct);
        return Created("Product created correctly.", null);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public IActionResult UpdateProduct([FromBody] UpdateProductDto product)
    {
        _productService.UpdateProduct(product);
        return Ok("Product updated correctly.");
    }

    [Authorize(Roles = "Admin, Client")]
    [HttpGet]
    public IActionResult GetProducts([FromQuery] ProductFilterDto filter)
    {
        var products = _productService.GetProducts(filter);
        return Ok(products);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("most-requested")]
    public IActionResult GetMostRequestedProducts([FromQuery] DateRangeDto dates)
    {
        var products = _productService.GetMostRequestedProducts(dates);
        return Ok(products);
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateStatus(int id, [FromBody] ProductStatusDto status)
    {
        _productService.UpdateStatus(id, status);
        return Ok("Product status updated correctly.");
    }
}
