using B3.Invest.Domain.Constants;
using B3.Invest.Domain.Entities;
using B3.Invest.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace B3.Invest.Domain.Services;

public class CalculatorService(IOptions<InvestmentTaxSettings> taxSettings) : ICalculatorService
{
    private readonly IOptions<InvestmentTaxSettings> _taxSettings = taxSettings;

    public decimal CalculateGross(Investment investment)
    {
        decimal amount = investment.InitialAmount;

        for (int i = 0; i < investment.Months; i++)
        {
            amount *= 1 + (CalculatorSettings.CDI * CalculatorSettings.TB);
        }

        return Math.Round(amount, 2);
    }

    public decimal CalculateNet(Investment investment)
    {
        var gross = CalculateGross(investment);
        var taxRate = GetTaxRate(investment.Months);

        var profit = gross - investment.InitialAmount;
        var tax = profit * taxRate;

        var net = gross - tax;
        return Math.Round(net, 2);
    }

    private decimal GetTaxRate(int months)
    {
        if (months <= _taxSettings.Value.MaxMonthsFirstRange)
            return _taxSettings.Value.TaxRateFirstRange;
        if (months <= _taxSettings.Value.MaxMonthsSecondRange)
            return _taxSettings.Value.TaxRateSecondRange;
        if (months <= _taxSettings.Value.MaxMonthsThirdRange)
            return _taxSettings.Value.TaxRateThirdRange;
        else
            return _taxSettings.Value.TaxRateDefault;
    }
}
