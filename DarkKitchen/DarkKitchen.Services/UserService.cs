using DarkKitchen.Domain.DataAccess.Interfaces;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Interfaces;
using DarkKitchen.Models.UserDTOs;

namespace DarkKitchen.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public IUserRepository UserRepository => _userRepository;

    public void CreateUser(UserDto newUser)
    {
        var user = new User
        {
            Name = newUser.Name,
            Surname = newUser.Surname,
            Email = newUser.Email,
            Phone = newUser.Phone,
            Password = newUser.Password,
            Role = Role.Client,
        };
        _userRepository.Add(user);
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
