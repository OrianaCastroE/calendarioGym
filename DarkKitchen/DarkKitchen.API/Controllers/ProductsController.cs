using DarkKitchen.Domain.Interfaces.Service;
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

    [Authorize(Policy = "CreateProduct")]
    [HttpPost]
    public IActionResult CreateProduct([FromBody] CreateProductDto newProduct)
    {
        _productService.CreateProduct(newProduct);
        return Created("Product created correctly.", null);
    }

    [Authorize(Policy = "UpdateProduct")]
    [HttpPut]
    public IActionResult UpdateProduct([FromBody] UpdateProductDto product)
    {
        _productService.UpdateProduct(product);
        return Ok("Product updated correctly.");
    }

    [Authorize(Policy = "GetProducts")]
    [HttpGet]
    public IActionResult GetProducts([FromQuery] ProductFilterDto filter)
    {
        var products = _productService.GetProducts(filter);
        return Ok(products);
    }

    [Authorize(Policy = "GetMostPopularProducts")]
    [HttpGet("most-requested")]
    public IActionResult GetMostRequestedProducts([FromQuery] DateRangeDto dates)
    {
        var products = _productService.GetMostRequestedProducts(dates);
        return Ok(products);
    }

    [Authorize(Policy = "UpdateProductStatus")]
    [HttpPatch("{id}")]
    public IActionResult UpdateStatus(int id, [FromBody] ProductStatusDto status)
    {
        _productService.UpdateStatus(id, status);
        return Ok("Product status updated correctly.");
    }
}
