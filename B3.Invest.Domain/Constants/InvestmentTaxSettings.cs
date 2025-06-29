namespace B3.Invest.Domain.Constants;

public class InvestmentTaxSettings
{
    public int MaxMonthsFirstRange { get; set; }
    public int MaxMonthsSecondRange { get; set; }
    public int MaxMonthsThirdRange { get; set; }
    public decimal TaxRateFirstRange { get; set; }
    public decimal TaxRateSecondRange { get; set; }
    public decimal TaxRateThirdRange { get; set; }
    public decimal TaxRateDefault { get; set; }
}
