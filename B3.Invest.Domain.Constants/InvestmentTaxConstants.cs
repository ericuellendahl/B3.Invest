namespace B3.Invest.Domain.Constants;

public static class InvestmentTaxConstants
{
    // Faixas de meses para tributação
    public const int MaxMonthsFirstRange = 6;
    public const int MaxMonthsSecondRange = 12;
    public const int MaxMonthsThirdRange = 24;

    // Alíquotas de imposto para cada faixa
    public const decimal TaxRateFirstRange = 0.225m;
    public const decimal TaxRateSecondRange = 0.20m;
    public const decimal TaxRateThirdRange = 0.175m;
    public const decimal TaxRateDefault = 0.15m;
}