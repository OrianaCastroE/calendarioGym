namespace DarkKitchen.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Password { get; set; }
    public Role Role { get; set; }
}
