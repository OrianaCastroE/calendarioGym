using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces.Repository;
using DarkKitchen.Domain.Interfaces.Service;
using DarkKitchen.Models.OrderDTOs;

namespace DarkKitchen.Services;

public class OrderService(IOrderRepository orderRepository) : IOrderService
{
    private readonly IOrderRepository orderRepository = orderRepository;

    public OrderResponseDto CreateOrder(OrderDto newOrder)
    {
        if(newOrder.products.Count == 0)
        {
            throw new Exception("Order must have at least one product.");
        }

        if(newOrder.deliveryType != "express" && newOrder.deliveryType != "24hs")
        {
            throw new Exception("Invalid delivery type.");
        }

        if(string.IsNullOrEmpty(newOrder.address.street))
        {
            throw new Exception("Street cannot be empty.");
        }

        if(string.IsNullOrEmpty(newOrder.address.doorNumber))
        {
            throw new Exception("Door number cannot be empty.");
        }

        var order = new Order()
        {
            DeliveryType = newOrder.deliveryType,
            Street = newOrder.address.street,
            DoorNumber = newOrder.address.doorNumber,
            Apartment = newOrder.address.apartment,
            Status = "Pending",
            CreatedAt = DateTime.Now
        };

        orderRepository.Add(order);

        return new OrderResponseDto(order.Id, order.ClientId, order.Status, order.CreatedAt, order.Subtotal, order.ShippingCost, order.Total, []);
    }

    public List<OrderResponseDto> GetClientOrders(int clientId, OrderFiltersDto filter)
    {
        var orders = orderRepository.GetClientOrders(clientId, filter.dateFrom, filter.dateTo, filter.status);

        return orders.Select(o => new OrderResponseDto(o.Id, o.ClientId, o.Status, o.CreatedAt, o.Subtotal, o.ShippingCost, o.Total, [])).ToList();
    }

    public List<OrderResponseDto> GetOrdersByStatus(OrderFilterByStatusDto filter)
    {
        var orders = orderRepository.GetOrdersByStatus(filter.dateFrom, filter.dateTo, filter.address, filter.status);

        return orders.Select(o => new OrderResponseDto(o.Id, o.ClientId, o.Status, o.CreatedAt, o.Subtotal, o.ShippingCost, o.Total, [])).ToList();
    }

    public OrderResponseDto GetOrderById(int orderId)
    {
        var order = orderRepository.GetById(orderId)
            ?? throw new Exception("Order not found.");

        return new OrderResponseDto(order.Id, order.ClientId, order.Status, order.CreatedAt, order.Subtotal, order.ShippingCost, order.Total, []);
    }

    public void UpdateOrderStatus(int orderId, UpdateOrderStatusDto newStatus)
    {
        var order = orderRepository.GetById(orderId)
            ?? throw new Exception("Order not found.");

        order.Status = newStatus.status!;

        orderRepository.Update(order);
    }
}
