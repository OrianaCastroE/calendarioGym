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

    [HttpGet("client/{clientId}")]
    public IActionResult GetClientOrders(int clientId, [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo, [FromQuery] string? status)
    {
        try
        {
            var orders = _orderService.GetClientOrders(clientId, dateFrom, dateTo, status);
            return Ok(orders);
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult GetOrdersByStatus([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo, [FromQuery] string? address, [FromQuery] string? status)
    {
        try
        {
            var orders = _orderService.GetOrdersByStatus(dateFrom, dateTo, address, status);
            return Ok(orders);
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
