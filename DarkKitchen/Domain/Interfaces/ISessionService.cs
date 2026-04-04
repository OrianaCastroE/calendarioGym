using Models.SessionDTOs;

namespace Domain.Interfaces;

public interface ISessionService
{
    public LoginResponseDto Login(LoginDto loginDto);
}
