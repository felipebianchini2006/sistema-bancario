namespace BankApi.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = "customer";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
