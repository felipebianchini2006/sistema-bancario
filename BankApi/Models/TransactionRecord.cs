namespace BankApi.Models;

public enum TxType { credit, debit }

public class TransactionRecord
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public TxType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Account? Account { get; set; }
}
