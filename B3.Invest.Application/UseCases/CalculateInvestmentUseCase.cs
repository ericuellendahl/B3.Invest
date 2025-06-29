using B3.Invest.Domain.DTOs;
using B3.Invest.Domain.Entities;
using B3.Invest.Domain.Interfaces;


namespace B3.Invest.Application.UseCases;

public class CalculateInvestmentUseCase(ICalculatorService calculator) : ICalculateInvestmentUseCase
{
    private readonly ICalculatorService _calculator = calculator;

    public InvestmentResponse Execute(InvestmentRequest investmentRequest)
    {
        var investment = new Investment(investmentRequest.InitialAmount, investmentRequest.Months);

        return new InvestmentResponse(
            _calculator.CalculateGross(investment),
            _calculator.CalculateNet(investment)
        );
    }
}
