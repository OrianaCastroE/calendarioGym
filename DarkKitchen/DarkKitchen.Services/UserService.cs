using DarkKitchen.Domain.DataAccess.Interfaces;
using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.UserDTOs;

namespace DarkKitchen.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository
        ?? throw new ArgumentNullException(nameof(userRepository));

    public IUserRepository UserRepository => _userRepository;

    public void CreateUser(UserDto newUser)
    {
    }

    public void UpdateUser(UserDto updatedUser)
    {
    }

    public List<UserResponseDto> GetUsers(string name, string surname)
    {
        return null;
    }

    public void DeleteUser(string email)
    {
    }

    // lo dejo para el final porque todavía no sé como implementarlo
    public void CreateUserWithRole(CreateUserDto newUser)
    {
    }
}
