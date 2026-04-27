using DarkKitchen.Domain.Entities;

namespace DarkKitchen.Domain.Interfaces.Repository;

public interface IOrderRepository
{
    void Add(Order order);
    Order? GetById(int id);
    IEnumerable<Order> GetAll();
    IEnumerable<Order> GetClientOrders(int clientId, DateTime? dateFrom, DateTime? dateTo, string? status);
    IEnumerable<Order> GetOrdersByStatus(DateTime dateFrom, DateTime dateTo, string? address, string? status);
    void Update(Order order);
}
