#include "RiskEngine.h"
#include <algorithm>

double __cdecl CreditRiskScore(double income, double balance, int latePayments)
{
    if (income <= 0) income = 1.0;
    // utilização da renda
    double util = std::max(0.0, 1.0 - (balance / (income * 3.0)));
    // penalidade por atrasos
    double lateFactor = 1.0 + (latePayments * 0.15);
    double score = util * lateFactor;
    if (score > 1.0) score = 1.0;
    if (score < 0.0) score = 0.0;
    return score;
}
