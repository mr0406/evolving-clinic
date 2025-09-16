using EvolvingClinic.Domain.Shared;
using EvolvingClinic.Domain.Utils;

namespace EvolvingClinic.Domain.HealthcareServices;

public class HealthcareServiceType
{
    public string Code { get; }
    private string _name;
    private TimeSpan _duration;
    private Money _price;
    private List<PriceHistoryEntry> _priceHistory;

    private HealthcareServiceType(
        string name,
        string code,
        TimeSpan duration,
        Money price)
    {
        Code = code;
        _name = name;
        _duration = duration;
        _priceHistory = new List<PriceHistoryEntry>();
        ApplyPriceChange(price);
    }

    public static HealthcareServiceType Create(
        string name,
        string code,
        TimeSpan duration,
        Money price,
        List<string> existingNames,
        List<string> existingCodes)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Service name is required");
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Service code is required");
        }

        name = name.Trim();
        code = code.Trim().ToUpperInvariant();

        if (existingNames.Any(n => string.Equals(n, name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException($"A service with the name '{name}' already exists");
        }

        if (existingCodes.Any(c => string.Equals(c, code, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException($"A service with the code '{code}' already exists");
        }

        if (duration.TotalMinutes < 15)
        {
            throw new ArgumentException("Service duration must be at least 15 minutes");
        }

        if (duration.TotalHours > 8)
        {
            throw new ArgumentException("Service duration cannot exceed 8 hours");
        }

        return new HealthcareServiceType(
            name,
            code,
            duration,
            price);
    }

    public Snapshot CreateSnapshot()
    {
        return new Snapshot(
            Code,
            _name,
            _duration,
            _price,
            _priceHistory.AsReadOnly());
    }

    public void ChangePrice(Money newPrice)
    {
        ApplyPriceChange(newPrice);
    }

    private void ApplyPriceChange(Money newPrice)
    {
        if (newPrice.Value < 0)
        {
            throw new ArgumentException("Service price must be 0 or greater");
        }

        var today = ApplicationClock.Today;
        var todayEntry = _priceHistory.SingleOrDefault(e => e.EffectiveFrom == today);

        if (todayEntry != null)
        {
            var index = _priceHistory.IndexOf(todayEntry);
            _priceHistory[index] = todayEntry with { Price = newPrice };
            _price = newPrice;

            return;
        }

        var currentEntry = _priceHistory.SingleOrDefault(e => e.EffectiveTo == null);
        if (currentEntry != null)
        {
            var index = _priceHistory.IndexOf(currentEntry);
            _priceHistory[index] = currentEntry with { EffectiveTo = today.AddDays(-1) };
        }

        _priceHistory.Add(new PriceHistoryEntry(newPrice, today, null));
        _price = newPrice;
    }

    public record Snapshot(
        string Code,
        string Name,
        TimeSpan Duration,
        Money Price,
        IReadOnlyList<PriceHistoryEntry> PriceHistory);

    public record PriceHistoryEntry(Money Price, DateOnly EffectiveFrom, DateOnly? EffectiveTo);
}