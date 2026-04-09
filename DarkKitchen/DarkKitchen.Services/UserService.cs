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
        if(string.IsNullOrEmpty(newUser.Name))
        {
            throw new ArgumentException("Name cannot be empty or whitespace.");
        }

        if(string.IsNullOrEmpty(newUser.Surname))
        {
            throw new ArgumentException("Surname cannot be empty or whitespace.");
        }

        if(!newUser.Email.Contains("@"))
        {
            throw new ArgumentException("Email is not valid.");
        }

        if(newUser.Password.Length > 25)
        {
            throw new ArgumentException("Password cannot be longer than 25 characters.");
        }

        if(newUser.Password.Length < 15)
        {
            throw new ArgumentException("Password must be at least 15 characters long.");
        }

        if(!newUser.Password.Any(char.IsUpper))
        {
            throw new ArgumentException("Password must contain at least one uppercase letter.");
        }

        if(!newUser.Password.Any(char.IsLower))
        {
            throw new ArgumentException("Password must contain at least one lowercase letter.");
        }

        var specialChars = "!@#$%^&*()_+-=[]{};:,.<>?/~";
        if(!newUser.Password.Any(specialChars.Contains))
        {
            throw new ArgumentException("Password must contain at least one special character.");
        }

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
