using DarkKitchen.Models.OrderDTOs;

namespace DarkKitchen.Domain.Interfaces;

public interface IOrderService
{
    public OrderResponseDto CreateOrder(OrderDto newOrder);
    public List<OrderResponseDto> GetClientOrders(int clientId, DateTime? dateFrom, DateTime? dateTo, string? status);
    public List<OrderResponseDto> GetOrdersByStatus(DateTime dateFrom, DateTime dateTo, string? address, string? status);
    public OrderResponseDto GetOrderById(int orderId);
    public void UpdateOrderStatus(int orderId, UpdateOrderStatusDto newStatus);
}
