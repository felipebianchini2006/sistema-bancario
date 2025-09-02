using BankApi.Models;
namespace BankApi.Dto;
public record CreateTransactionRequest(int AccountId, TxType Type, decimal Amount, string? Description);
public record TransferRequest(int FromAccountId, int ToAccountId, decimal Amount, string? Description);
