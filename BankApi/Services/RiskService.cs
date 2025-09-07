namespace BankApi.Services;
public sealed class RiskService
{
    public (double score, string category) Assess(double income, double balance, int late)
    {
        var s = RiskInterop.CreditRiskScore(income, balance, late);
        var cat = s <= 0.20 ? "baixo" : s <= 0.50 ? "moderado" : s <= 0.80 ? "alto" : "crítico";
        return (Math.Round(s, 4), cat);
    }
}
