namespace EvolvingClinic.Domain.Shared;

public readonly record struct Money
{
    public decimal Value { get; }

    public Money(decimal value)
    {
        Value = Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }

    public override string ToString() => $"${Value:F2}";
}