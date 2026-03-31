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
        _productService.CreateProduct(newProduct);
        return Created("Product created correctly.", null);
    }
}
