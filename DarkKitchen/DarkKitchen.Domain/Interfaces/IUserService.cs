using DarkKitchen.Models.UserDTOs;

namespace DarkKitchen.Domain.Interfaces;

public interface IUserService
{
    public void CreateUserWithRole(CreateUserDto newUser);
    public void CreateUser(UserDto newUser);
    public void UpdateUser(UserDto updatedUser);
    public List<UserResponseDto> GetUsers(string name, string surname);
    public void DeleteUser(string email);
}
