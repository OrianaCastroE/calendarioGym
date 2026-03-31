// using Domain.DTOs.PrductDTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ProductsController(IProductService productService) : ControllerBase {
    private readonly IProductService _productService = productService;

    [HttpPost]
    public void CreateProduct([FromBody] CreateProductDto newProduct)
    {

    }

}
