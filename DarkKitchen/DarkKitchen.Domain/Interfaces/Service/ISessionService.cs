using DarkKitchen.Models.SessionDTOs;

namespace DarkKitchen.Domain.Interfaces.Service;

public interface ISessionService
{
    public LoginResponseDto Login(LoginDto loginDto);
}
