using B3.Invest.Domain.Constants;
using B3.Invest.Domain.Entities;
using B3.Invest.Domain.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace B3.Invest.Tests.DomainTests;

public class DomainTest
{
    [Fact]
    public void CalculateGross_ReturnsExpectedValue()
    {
        // Arrange
        var taxSettings = new InvestmentTaxSettings
        {
            MaxMonthsFirstRange = 6,
            MaxMonthsSecondRange = 12,
            MaxMonthsThirdRange = 24,
            TaxRateFirstRange = 0.225m,
            TaxRateSecondRange = 0.20m,
            TaxRateThirdRange = 0.175m,
            TaxRateDefault = 0.15m
        };

        var optionsMock = new Mock<IOptions<InvestmentTaxSettings>>();
        optionsMock.Setup(o => o.Value).Returns(taxSettings);

        var calculator = new CalculatorService(optionsMock.Object);

        var investment = new Investment(1000m, 12);

        // Act
        var gross = calculator.CalculateGross(investment);

        // Assert
        Assert.True(gross > 1000m);
    }

    [Fact]
    public void CalculateNet_ReturnsExpectedValue()
    {
        // Arrange
        var taxSettings = new InvestmentTaxSettings
        {
            MaxMonthsFirstRange = 6,
            MaxMonthsSecondRange = 12,
            MaxMonthsThirdRange = 24,
            TaxRateFirstRange = 0.225m,
            TaxRateSecondRange = 0.20m,
            TaxRateThirdRange = 0.175m,
            TaxRateDefault = 0.15m
        };

        var optionsMock = new Mock<IOptions<InvestmentTaxSettings>>();
        optionsMock.Setup(o => o.Value).Returns(taxSettings);

        var calculator = new CalculatorService(optionsMock.Object);

        var investment = new Investment(1000m, 12);

        // Act
        var net = calculator.CalculateNet(investment);

        // Assert
        Assert.True(net > 0m && net < calculator.CalculateGross(investment));
    }
}
