using B3.Invest.Domain.DTOs;
using B3.Invest.Domain.Entities;
using B3.Invest.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace B3.Invest.Application.UseCases;

public class CalculateInvestmentUseCase(
    ICalculatorService calculator,
    ILogger<CalculateInvestmentUseCase> logger) : ICalculateInvestmentUseCase
{
    private readonly ICalculatorService _calculator = calculator;
    private readonly ILogger<CalculateInvestmentUseCase> _logger = logger;

    public InvestmentResponse Execute(InvestmentRequest investmentRequest)
    {
        try
        {
            _logger.LogInformation("Iniciando cálculo de investimento. Valor inicial: {InitialAmount}, Meses: {Months}",
                                    investmentRequest.InitialAmount, investmentRequest.Months);

            var investment = new Investment(investmentRequest.InitialAmount, investmentRequest.Months);

            return new InvestmentResponse(
                _calculator.CalculateGross(investment),
                _calculator.CalculateNet(investment)
            );
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro ao criar entidade Investment com valores inválidos: {InitialAmount}, {Months}",
                investmentRequest.InitialAmount, investmentRequest.Months);
            throw new ArgumentException("Invalid investment parameters provided.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao calcular investimento.");
            throw new InvalidOperationException("An unexpected error occurred while calculating the investment.", ex);
        }
    }
}
