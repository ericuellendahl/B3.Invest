namespace B3.Invest.Domain.Entities;

public record Investment
{
    public decimal InitialAmount { get; }
    public int Months { get; }

    public Investment(decimal initialAmount, int months)
    {
        if (initialAmount <= 0)
            throw new ArgumentException("Valor inicial deve ser maior que zero.", nameof(initialAmount));

        if (months < 2)
            throw new ArgumentException("Meses deve ser maior ou igual a 2.", nameof(months));

        InitialAmount = initialAmount;
        Months = months;
    }
}
