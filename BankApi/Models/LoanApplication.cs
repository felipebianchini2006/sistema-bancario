namespace BankApi.Models;

public class LoanApplication
{
    public int Id { get; set; }
    public int? AccountId { get; set; }
    public double Income { get; set; }
    public double Balance { get; set; }
    public int LatePayments { get; set; }
    public double Requested { get; set; }
    public double Score { get; set; }
    public string Category { get; set; } = "";
    public bool Approved { get; set; }
    public double MaxAmount { get; set; }
    public double? SuggestedApr { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? UserId { get; set; }
}
