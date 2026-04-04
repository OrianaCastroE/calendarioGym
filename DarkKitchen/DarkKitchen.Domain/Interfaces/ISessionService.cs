using DarkKitchen.Models.SessionDTOs;

namespace DarkKitchen.Domain.Interfaces;

public interface ISessionService
{
    public LoginResponseDto Login(LoginDto loginDto);
}
