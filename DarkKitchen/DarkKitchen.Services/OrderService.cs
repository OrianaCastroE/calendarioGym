using DarkKitchen.Domain.DataAccess.Interfaces;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.OrderDTOs;

namespace DarkKitchen.Services;

public class OrderService(
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IPromotionRepository promotionRepository) : IOrderService
{
    private readonly IOrderRepository orderRepository = orderRepository;
    private readonly IProductRepository productRepository = productRepository;
    private readonly IPromotionRepository promotionRepository = promotionRepository;

    public OrderResponseDto CreateOrder(OrderDto newOrder)
    {
        if(newOrder.Products.Count == 0)
            throw new Exception("Order must have at least one product.");

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
        throw new NotImplementedException();
    }

    public List<OrderResponseDto> GetOrdersByStatus(DateTime dateFrom, DateTime dateTo, string? address, string? status)
    {
        throw new NotImplementedException();
    }

    public OrderResponseDto GetOrderById(int orderId)
    {
        throw new NotImplementedException();
    }

    public void UpdateOrderStatus(int orderId, UpdateOrderStatusDto newStatus)
    {
        throw new NotImplementedException();
    }
}
