using B3.Invest.Application.UseCases;
using B3.Invest.Application.Validators;
using B3.Invest.Domain.Constants;
using B3.Invest.Domain.DTOs;
using B3.Invest.Domain.Interfaces;
using B3.Invest.Domain.Services;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace B3.Invest.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddTransient<IValidator<InvestmentRequest>, InvestmentValidator>();
        return services;
    }

    public static IServiceCollection AddIoC(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICalculatorService, CalculatorService>();
        services.AddScoped<ICalculateInvestmentUseCase, CalculateInvestmentUseCase>();
        services.Configure<InvestmentTaxSettings>(configuration.GetSection("InvestmentTaxSettings")); 
        return services;
    }
}
