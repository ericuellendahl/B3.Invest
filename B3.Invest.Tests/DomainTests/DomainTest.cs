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

    [Fact]
    public void Constants_ShouldHaveExpectedValues()
    {
        // Acesso direto às constantes para garantir cobertura
        var cdi = CalculatorSettings.CDI;
        var tb = CalculatorSettings.TB;

        Assert.Equal(0.009M, cdi);
        Assert.Equal(1.08M, tb);
    }

    [Fact]
    public void Investment_WithMonthsLessThanTwo_ThrowsArgumentException()
    {
        // Arrange
        decimal initialAmount = 1000m;
        int invalidMonths = 1;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Investment(initialAmount, invalidMonths));
        Assert.Equal("Meses deve ser maior ou igual a 2. (Parameter 'months')", ex.Message);
    }

    [Fact]
    public void GetTaxRate_ReturnsExpectedRates_ForAllRanges()
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

        // Use reflection to access the private method
        var method = typeof(CalculatorService).GetMethod("GetTaxRate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        Assert.NotNull(method);

        // Act & Assert for each range
        Assert.Equal(0.225m, method.Invoke(calculator, new object[] { 1 }));
        Assert.Equal(0.225m, method.Invoke(calculator, new object[] { 6 }));
        Assert.Equal(0.20m, method.Invoke(calculator, new object[] { 7 }));
        Assert.Equal(0.20m, method.Invoke(calculator, new object[] { 12 }));
        Assert.Equal(0.175m, method.Invoke(calculator, new object[] { 13 }));
        Assert.Equal(0.175m, method.Invoke(calculator, new object[] { 24 }));
        Assert.Equal(0.15m, method.Invoke(calculator, new object[] { 25 }));
        Assert.Equal(0.15m, method.Invoke(calculator, new object[] { 100 }));
    }
}
