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

    [HttpGet("client/{clientId}")]
    public IActionResult GetClientOrders(int clientId, [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo, [FromQuery] string? status)
    {
        var orders = _orderService.GetClientOrders(clientId, dateFrom, dateTo, status);
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

    [HttpPut("{orderId}/prepare")]
    public IActionResult PrepareOrder(int orderId)
    {
        _orderService.PrepareOrder(orderId);
        return Ok("Order prepared correctly.");
    }

    [HttpPut("{orderId}/cancel")]
    public IActionResult CancelOrder(int orderId)
    {
        _orderService.CancelOrder(orderId);
        return Ok("Order cancelled correctly.");
    }

    [HttpPut("{orderId}/on-the-way")]
    public IActionResult SetOrderOnTheWay(int orderId)
    {
        _orderService.SetOrderOnTheWay(orderId);
        return Ok("Order is on the way.");
    }

    [HttpPut("{orderId}/deliver")]
    public IActionResult DeliverOrder(int orderId)
    {
        _orderService.DeliverOrder(orderId);
        return Ok("Order delivered correctly.");
    }

    [HttpPut("{orderId}/not-delivered")]
    public IActionResult SetOrderNotDelivered(int orderId)
    {
        _orderService.SetOrderNotDelivered(orderId);
        return Ok("Order marked as not delivered.");
    }
}
