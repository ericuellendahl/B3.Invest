using B3.Invest.Application.UseCases;
using B3.Invest.Application.Validators;
using B3.Invest.Domain.DTOs;
using B3.Invest.Domain.Entities;
using B3.Invest.Domain.Interfaces;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace B3.Invest.Tests.ApplicationTests;

public class ApplicationTest
{
    [Fact]
    public void Execute_ReturnsExpectedResponse()
    {
        // Arrange
        var mockCalculator = new Mock<ICalculatorService>();
        var mockLogger = new Mock<ILogger<CalculateInvestmentUseCase>>();

        mockCalculator.Setup(c => c.CalculateGross(It.IsAny<Investment>()))
            .Returns(1200m);
        mockCalculator.Setup(c => c.CalculateNet(It.IsAny<Investment>()))
            .Returns(1000m);

        var useCase = new CalculateInvestmentUseCase(mockCalculator.Object, mockLogger.Object);
        var request = new InvestmentRequest(1000m, 12);

        // Act
        var response = useCase.Execute(request);

        // Assert
        Assert.Equal(1200m, response.GrossAmount);
        Assert.Equal(1000m, response.NetAmount);

        mockCalculator.Verify(c => c.CalculateGross(
            It.Is<Investment>(i => i.InitialAmount == 1000m && i.Months == 12)), Times.Once);
        mockCalculator.Verify(c => c.CalculateNet(
            It.Is<Investment>(i => i.InitialAmount == 1000m && i.Months == 12)), Times.Once);
    }

    [Fact]
    public void Execute_InvalidInvestment_ThrowsArgumentExceptionAndLogsWarning()
    {
        // Arrange
        var mockCalculator = new Mock<ICalculatorService>();
        var mockLogger = new Mock<ILogger<CalculateInvestmentUseCase>>();

        // Simulate ArgumentException when creating Investment entity
        var useCase = new CalculateInvestmentUseCase(mockCalculator.Object, mockLogger.Object);
        var invalidRequest = new InvestmentRequest(-1000m, -5); // Example of invalid values

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => useCase.Execute(invalidRequest));
        Assert.Equal("Invalid investment parameters provided.", ex.Message);

        mockLogger.Verify(
            l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Erro ao criar entidade Investment com valores inválidos")),
                It.IsAny<ArgumentException>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public void Execute_CalculatorThrowsException_ThrowsInvalidOperationExceptionAndLogsError()
    {
        // Arrange
        var mockCalculator = new Mock<ICalculatorService>();
        var mockLogger = new Mock<ILogger<CalculateInvestmentUseCase>>();

        // Simulate exception in calculator
        mockCalculator.Setup(c => c.CalculateGross(It.IsAny<Investment>()))
            .Throws(new Exception("Calculation failed"));

        var useCase = new CalculateInvestmentUseCase(mockCalculator.Object, mockLogger.Object);
        var request = new InvestmentRequest(1000m, 12);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => useCase.Execute(request));
        Assert.Equal("An unexpected error occurred while calculating the investment.", ex.Message);

        mockLogger.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Erro inesperado ao calcular investimento.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}

public class InvestmentValidatorTests
{
    [Fact]
    public void Validate_ValidRequest_ReturnsSuccess()
    {
        // Arrange
        var validator = new InvestmentValidator();
        var request = new InvestmentRequest(1000m, 12);

        // Act
        ValidationResult result = validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0, 12, "Valor inicial inválido")]
    [InlineData(-100, 12, "Valor inicial inválido")]
    [InlineData(1000, 1, "Meses deve ser maior ou igual a 2")]
    [InlineData(1000, 0, "Meses deve ser maior ou igual a 2")]
    public void Validate_InvalidRequest_ReturnsFailure(decimal initialAmount, int months, string expectedMessage)
    {
        // Arrange
        var validator = new InvestmentValidator();
        var request = new InvestmentRequest(initialAmount, months);

        // Act
        ValidationResult result = validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == expectedMessage);
    }
}
