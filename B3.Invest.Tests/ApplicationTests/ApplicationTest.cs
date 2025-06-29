using B3.Invest.Application.UseCases;
using B3.Invest.Domain.DTOs;
using B3.Invest.Domain.Entities;
using B3.Invest.Domain.Interfaces;
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
}
