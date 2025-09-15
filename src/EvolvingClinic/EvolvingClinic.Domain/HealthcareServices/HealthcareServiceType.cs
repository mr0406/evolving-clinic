using EvolvingClinic.Domain.Shared;

namespace EvolvingClinic.Domain.HealthcareServices;

public class HealthcareServiceType
{
    public string Code { get; }
    private string _name;
    private TimeSpan _duration;
    private Money _price;

    private HealthcareServiceType(
        string name,
        string code,
        TimeSpan duration,
        Money price)
    {
        Code = code;
        _name = name;
        _duration = duration;
        _price = price;
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

        if (price.Value < 0)
        {
            throw new ArgumentException("Service price must be 0 or greater");
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
            _price);
    }

    public record Snapshot(
        string Code,
        string Name,
        TimeSpan Duration,
        Money Price);
}