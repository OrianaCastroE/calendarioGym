using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.OrderDTOs;

namespace DarkKitchen.Services;

public class OrderService(IOrderRepository orderRepository) : IOrderService
{
    private readonly IOrderRepository orderRepository = orderRepository;

    public OrderResponseDto CreateOrder(OrderDto newOrder)
    {
        if(newOrder.Products.Count == 0)
        {
            throw new Exception("Order must have at least one product.");
        }

        if(newOrder.DeliveryType != "express" && newOrder.DeliveryType != "24hs")
        {
            throw new Exception("Invalid delivery type.");
        }

        if(string.IsNullOrEmpty(newOrder.Address.Street))
        {
            throw new Exception("Street cannot be empty.");
        }

        if(string.IsNullOrEmpty(newOrder.Address.DoorNumber))
        {
            throw new Exception("Door number cannot be empty.");
        }

        var order = new Order()
        {
            DeliveryType = newOrder.DeliveryType,
            Street = newOrder.Address.Street,
            DoorNumber = newOrder.Address.DoorNumber,
            Apartment = newOrder.Address.Apartment,
            Status = "Pending",
            CreatedAt = DateTime.Now
        };

        orderRepository.Add(order);
        orderRepository.Save();

        return new OrderResponseDto()
        {
            Id = order.Id,
            Status = order.Status,
            CreatedAt = order.CreatedAt
        };
    }

    public List<OrderResponseDto> GetClientOrders(int clientId, DateTime? dateFrom, DateTime? dateTo, string? status)
    {
        var orders = orderRepository.GetClientOrders(clientId, dateFrom, dateTo, status);

        return orders.Select(o => new OrderResponseDto()
        {
            Id = o.Id,
            ClientId = o.ClientId,
            Status = o.Status,
            CreatedAt = o.CreatedAt,
            Subtotal = o.Subtotal,
            ShippingCost = o.ShippingCost,
            Total = o.Total
        }).ToList();
    }

    public List<OrderResponseDto> GetOrdersByStatus(DateTime dateFrom, DateTime dateTo, string? address, string? status)
    {
        var orders = orderRepository.GetOrdersByStatus(dateFrom, dateTo, address, status);

        return orders.Select(o => new OrderResponseDto()
        {
            Id = o.Id,
            ClientId = o.ClientId,
            Status = o.Status,
            CreatedAt = o.CreatedAt,
            Subtotal = o.Subtotal,
            ShippingCost = o.ShippingCost,
            Total = o.Total
        }).ToList();
    }

    public OrderResponseDto GetOrderById(int orderId)
    {
        var order = orderRepository.GetById(orderId)
            ?? throw new Exception("Order not found.");

        return new OrderResponseDto()
        {
            Id = order.Id,
            ClientId = order.ClientId,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            Subtotal = order.Subtotal,
            ShippingCost = order.ShippingCost,
            Total = order.Total
        };
    }

    public void UpdateOrderStatus(int orderId, UpdateOrderStatusDto newStatus)
    {
        var order = orderRepository.GetById(orderId)
            ?? throw new Exception("Order not found.");

        order.Status = newStatus.Status!;

        orderRepository.Update(order);
        orderRepository.Save();
    }
}
