using B3.Invest.Domain.DTOs;
using FluentValidation;

namespace B3.Invest.Application.Validators;

public class InvestmentValidator : AbstractValidator<InvestmentRequest>
{
    public InvestmentValidator()
    {
        RuleFor(x => x.InitialAmount)
            .GreaterThan(0).WithMessage("Valor inicial inválido");
        RuleFor(x => x.Months)
            .GreaterThanOrEqualTo(2).WithMessage("Meses deve ser maior ou igual a 2");
    }
}