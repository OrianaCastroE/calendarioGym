using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces.Repository;

namespace DarkKitchen.DataAccess.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public void Add(User user)
    {
        context.Users.Add(user);
        context.SaveChanges();
    }

    public void Delete(User user)
    {
        throw new NotImplementedException();
    }

    public User? GetByEmail(string email)
    {
        return context.Users.FirstOrDefault(u => u.Email == email);
    }

    public List<User> GetUsers(string? name, string? surname)
    {
        throw new NotImplementedException();
    }

    public void Update(User user)
    {
        throw new NotImplementedException();
    }
}
