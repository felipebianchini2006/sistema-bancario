namespace BankApi.Dto;
public record CreateAccountRequest(int UserId, string Number, decimal InitialBalance);
public record AccountView(int Id, string Number, decimal Balance, int UserId);
