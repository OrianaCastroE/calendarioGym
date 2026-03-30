namespace Domain.Interfaces;

public interface IUserService
{
    public bool UserExists(string email);
}
