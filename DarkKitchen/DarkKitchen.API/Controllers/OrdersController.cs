using System.Security.Claims;
using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DarkKitchen.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [Authorize(Roles = "Client")]
    [HttpPost]
    public IActionResult CreateOrder([FromBody] OrderDto newOrder)
    {
        var order = _orderService.CreateOrder(newOrder);
        return Created(string.Empty, order);
    }

    [Authorize(Roles = "Client")]
    [HttpGet("client")]
    public IActionResult GetClientOrders([FromQuery] OrderFiltersDto filter)
    {
        var clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var orders = _orderService.GetClientOrders(clientId, filter);
        return Ok(orders);
    }

    [Authorize(Roles = "Dispatcher")]
    [HttpGet]
    public IActionResult GetOrdersByStatus([FromQuery] OrderFilterByStatusDto filter)
    {
        var orders = _orderService.GetOrdersByStatus(filter);
        return Ok(orders);
    }

    [Authorize(Roles = "Admin, Dispatcher")]
    [HttpGet("{orderId}")]
    public IActionResult GetOrderById(int orderId)
    {
        var order = _orderService.GetOrderById(orderId);
        return Ok(order);
    }

    [Authorize(Roles = "Admin, Dispatcher")]
    [HttpPatch("{orderId}/status")]
    public IActionResult UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto newStatus)
    {
        // TODO: Add role validation in service layer to ensure only allowed roles can update to certain statuses
        _orderService.UpdateOrderStatus(orderId, newStatus);
        return Ok("Order status updated.");
    }
}
