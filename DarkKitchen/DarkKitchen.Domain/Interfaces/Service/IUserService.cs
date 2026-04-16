using DarkKitchen.Models.UserDTOs;

namespace DarkKitchen.Domain.Interfaces.Service;

public interface IUserService
{
    public void CreateUserWithRole(CreateUserDto newUser);
    public void CreateUser(UserDto newUser);
    public void UpdateUser(UserDto updatedUser);
    public List<UserResponseDto> GetUsers(UserFiltersDto filter);
    public void DeleteUser(string email);
}
