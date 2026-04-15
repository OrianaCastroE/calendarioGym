using DarkKitchen.Models.OrderDTOs;

namespace DarkKitchen.Domain.Interfaces;

public interface IOrderService
{
    public OrderResponseDto CreateOrder(OrderDto newOrder);
    public List<OrderResponseDto> GetClientOrders(int clientId, OrderFiltersDto filter);
    public List<OrderResponseDto> GetOrdersByStatus(OrderFilterByStatusDto filter);
    public OrderResponseDto GetOrderById(int orderId);
    public void UpdateOrderStatus(int orderId, UpdateOrderStatusDto newStatus);
}
