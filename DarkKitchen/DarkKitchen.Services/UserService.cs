using DarkKitchen.Domain.Entities;
using DarkKitchen.Domain.Enums;
using DarkKitchen.Domain.Exceptions;
using DarkKitchen.Domain.Interfaces.Repository;
using DarkKitchen.Domain.Interfaces.Service;
using DarkKitchen.Models.UserDTOs;

namespace DarkKitchen.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public IUserRepository UserRepository => _userRepository;

    public void CreateUser(UserDto newUser)
    {
        if(string.IsNullOrEmpty(newUser.name))
        {
            throw new NameEmptyException();
        }

        if(string.IsNullOrEmpty(newUser.surname))
        {
            throw new SurnameEmptyException();
        }

        if(!newUser.email.Contains('@'))
        {
            throw new InvalidEmailException();
        }

        if(newUser.password.Length > 25)
        {
            throw new PasswordTooLongException();
        }

        if(newUser.password.Length < 15)
        {
            throw new PasswordTooShortException();
        }

        if(!newUser.password.Any(char.IsUpper))
        {
            throw new PasswordMissingUppercaseException();
        }

        if(!newUser.password.Any(char.IsLower))
        {
            throw new PasswordMissingLowercaseException();
        }

        var specialChars = "!@#$%^&*()_+-=[]{};:,.<>?/~";
        if(!newUser.password.Any(specialChars.Contains))
        {
            throw new PasswordMissingSpecialCharacterException();
        }

        if(!newUser.password.Any(char.IsDigit))
        {
            throw new PasswordMissingNumberException();
        }

        var user = new User
        {
            Name = newUser.name,
            Surname = newUser.surname,
            Email = newUser.email,
            Phone = newUser.phone,
            Password = newUser.password,
            Role = Role.Client,
        };

        _userRepository.Add(user);
    }

    public void UpdateUser(UserDto updatedUser)
    {
        if(string.IsNullOrWhiteSpace(updatedUser.email))
        {
            throw new EmailEmptyException();
        }

        User user = _userRepository.GetByEmail(updatedUser.email!)
            ?? throw new UserNotFoundException();

        if(updatedUser.name != null)
        {
            user.Name = updatedUser.name;
        }

        if(updatedUser.surname != null)
        {
            user.Surname = updatedUser.surname;
        }

        if(updatedUser.phone != null)
        {
            user.Phone = updatedUser.phone;
        }

        if(updatedUser.password != null)
        {
            user.Password = updatedUser.password;
        }

        _userRepository.Update(user);
    }

    public List<UserResponseDto> GetUsers(UserFiltersDto filter)
    {
        List<User> users = _userRepository.GetUsers(filter.name, filter.surname);

        var result = users.Select(user => new UserResponseDto(user.Id, user.Name, user.Surname, user.Email, user.Phone, user.Role.ToString())).ToList();

        return result;
    }

    public void DeleteUser(string email)
    {
        if(string.IsNullOrWhiteSpace(email))
        {
            throw new EmailEmptyException();
        }

        User? user = _userRepository.GetByEmail(email);

        if(user == null)
        {
            throw new UserNotFoundException();
        }

        _userRepository.Delete(user);
    }

    public void CreateUserWithRole(CreateUserDto newUser)
    {
        throw new NotImplementedException();
    }
}
