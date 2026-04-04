using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.OrderDTOs;
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

    [HttpGet("{orderId}")]
    public IActionResult GetOrderById(int orderId)
    {
        try
        {
            var order = _orderService.GetOrderById(orderId);
            return Ok(order);
        }
        catch(Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{orderId}/prepare")]
    public IActionResult PrepareOrder(int orderId)
    {
        try
        {
            _orderService.PrepareOrder(orderId);
            return Ok("Order prepared correctly.");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{orderId}/cancel")]
    public IActionResult CancelOrder(int orderId)
    {
        try
        {
            _orderService.CancelOrder(orderId);
            return Ok("Order cancelled correctly.");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{orderId}/on-the-way")]
    public IActionResult SetOrderOnTheWay(int orderId)
    {
        try
        {
            _orderService.SetOrderOnTheWay(orderId);
            return Ok("Order is on the way.");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{orderId}/deliver")]
    public IActionResult DeliverOrder(int orderId)
    {
        try
        {
            _orderService.DeliverOrder(orderId);
            return Ok("Order delivered correctly.");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{orderId}/not-delivered")]
    public IActionResult SetOrderNotDelivered(int orderId)
    {
        try
        {
            _orderService.SetOrderNotDelivered(orderId);
            return Ok("Order marked as not delivered.");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
