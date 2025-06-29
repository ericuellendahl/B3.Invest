using B3.Invest.Domain.DTOs;

namespace B3.Invest.Domain.Interfaces
{
    public interface ICalculateInvestmentUseCase
    {
        InvestmentResponse Execute(InvestmentRequest investmentRequest);
    }
}
