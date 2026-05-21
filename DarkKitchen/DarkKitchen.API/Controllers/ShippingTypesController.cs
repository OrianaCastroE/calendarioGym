using DarkKitchen.Domain.Enums;
using DarkKitchen.Domain.Interfaces.Service;
using DarkKitchen.Models.ShippingTypeDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[ApiController]
[Route("api/shippingtypes")]
public class ShippingTypesController(IShippingTypeService shippingTypeService) : ControllerBase
{
    private readonly IShippingTypeService _shippingTypeService = shippingTypeService;

    [HttpGet]
    public IActionResult GetAll()
    {
        var shippingTypes = _shippingTypeService.GetAll();
        return Ok(shippingTypes);
    }

    public IActionResult GetById(int id)
    {
        throw new NotImplementedException();
    }

    public IActionResult Create([FromBody] ShippingTypeDto dto)
    {
        throw new NotImplementedException();
    }

    public IActionResult Update(int id, [FromBody] ShippingTypeDto dto)
    {
        throw new NotImplementedException();
    }
}
