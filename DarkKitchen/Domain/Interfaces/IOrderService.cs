using Models.OrderDTOs;

namespace Domain.Interfaces;

public interface IOrderService
{
    public OrderResponseDto CreateOrder(OrderDto newOrder);
    public List<OrderResponseDto> GetClientOrders(int clientId, DateTime? dateFrom, DateTime? dateTo, string? status);
    public List<OrderResponseDto> GetOrdersByStatus(DateTime dateFrom, DateTime dateTo, string? address, string? status);
    public OrderResponseDto GetOrderById(int orderId);
    public void PrepareOrder(int orderId);
    public void CancelOrder(int orderId);
    public void SetOrderOnTheWay(int orderId);
    public void DeliverOrder(int orderId);
    public void SetOrderNotDelivered(int orderId);
}
