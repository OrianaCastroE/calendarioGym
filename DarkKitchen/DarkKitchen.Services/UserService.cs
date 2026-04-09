using DarkKitchen.Domain.DataAccess.Interfaces;
using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Exceptions;
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
            throw new NameEmptyException();
        }

        if(string.IsNullOrEmpty(newUser.Surname))
        {
            throw new SurnameEmptyException();
        }

        if(!newUser.Email.Contains("@"))
        {
            throw new InvalidEmailException();
        }

        if(newUser.Password.Length > 25)
        {
            throw new PasswordTooLongException();
        }

        if(newUser.Password.Length < 15)
        {
            throw new PasswordTooShortException();
        }

        if(!newUser.Password.Any(char.IsUpper))
        {
            throw new PasswordMissingUppercaseException();
        }

        if(!newUser.Password.Any(char.IsLower))
        {
            throw new PasswordMissingLowercaseException();
        }

        var specialChars = "!@#$%^&*()_+-=[]{};:,.<>?/~";
        if(!newUser.Password.Any(specialChars.Contains))
        {
            throw new PasswordMissingSpecialCharacterException();
        }

        if(!newUser.Password.Any(char.IsDigit))
        {
            throw new PasswordMissingNumberException();
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
        if(string.IsNullOrWhiteSpace(updatedUser.Email))
        {
            throw new EmailEmptyException();
        }

        User user = _userRepository.GetByEmail(updatedUser.Email!)
            ?? throw new UserNotFoundException();

        user.Name = updatedUser.Name;
        user.Surname = updatedUser.Surname;
        user.Phone = updatedUser.Phone;
        user.Password = updatedUser.Password;

        _userRepository.Update(user);
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
