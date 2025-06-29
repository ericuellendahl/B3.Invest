using B3.Invest.Domain.Entities;

namespace B3.Invest.Domain.Interfaces;

public interface ICalculatorService
{
    decimal CalculateGross(Investment investment);
    decimal CalculateNet(Investment investment);
}
