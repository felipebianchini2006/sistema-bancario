#pragma once
extern "C" {
    __declspec(dllexport) double __cdecl CreditRiskScore(double income, double balance, int latePayments);
}
