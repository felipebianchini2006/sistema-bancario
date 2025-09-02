namespace BankApi.Models;

public class Account
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Number { get; set; } = null!;
    public decimal Balance { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public User? User { get; set; }
}
