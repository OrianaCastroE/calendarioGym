using Domain.DTOs.OrderDTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpPost]
    public IActionResult CreateOrder([FromBody] OrderDto newOrder)
    {
        try
        {
            var order = _orderService.CreateOrder(newOrder);
            return Created(string.Empty, order);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
