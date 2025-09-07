using System.Runtime.InteropServices;

namespace BankApi.Services;

public static class RiskInterop
{
    // Coloque a RiskEngine.dll no mesmo diretório do .exe da API
    private const string Dll = "RiskEngine.dll";

    [DllImport(Dll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreditRiskScore")]
    private static extern double CreditRiskScoreNative(double income, double balance, int latePayments);

    public static double CreditRiskScore(double income, double balance, int latePayments)
    {
        if (income < 0) income = 0;
        if (balance < 0) balance = 0;
        if (latePayments < 0) latePayments = 0;
        return CreditRiskScoreNative(income, balance, latePayments);
    }
}
