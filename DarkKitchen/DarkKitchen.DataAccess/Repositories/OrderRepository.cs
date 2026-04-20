using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces.Repository;

namespace DarkKitchen.DataAccess.Repositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public void Add(Order order)
    {
        throw new NotImplementedException();
    }

    public Order? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Order> GetClientOrders(int clientId, DateTime? dateFrom, DateTime? dateTo, string? status)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Order> GetOrdersByStatus(DateTime dateFrom, DateTime dateTo, string? address, string? status)
    {
        throw new NotImplementedException();
    }

    public void Update(Order order)
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }
}
