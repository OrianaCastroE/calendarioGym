using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.OrderDTOs;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpPost]
    public IActionResult CreateOrder([FromBody] OrderDto newOrder)
    {
        var order = _orderService.CreateOrder(newOrder);
        return Created(string.Empty, order);
    }

    [HttpGet("client")]
    public IActionResult GetClientOrders([FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo, [FromQuery] string? status)
    {
        // TODO: extract clientId from token (as in some other controllers)
        var orders = _orderService.GetClientOrders(0, dateFrom, dateTo, status);
        return Ok(orders);
    }

    [HttpGet]
    public IActionResult GetOrdersByStatus([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo, [FromQuery] string? address, [FromQuery] string? status)
    {
        var orders = _orderService.GetOrdersByStatus(dateFrom, dateTo, address, status);
        return Ok(orders);
    }

    [HttpGet("{orderId}")]
    public IActionResult GetOrderById(int orderId)
    {
        var order = _orderService.GetOrderById(orderId);
        return Ok(order);
    }

    [HttpPatch("{orderId}/status")]
    public IActionResult UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto newStatus)
    {
        _orderService.UpdateOrderStatus(orderId, newStatus);
        return Ok("Order status updated.");
    }
}
