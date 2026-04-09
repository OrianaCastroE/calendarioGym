using DarkKitchen.Domain.Entities;

namespace DarkKitchen.Domain.DataAccess.Interfaces;

public interface IUserRepository
{
    public User GetByEmail(string email);
    User Add(User user);
}
