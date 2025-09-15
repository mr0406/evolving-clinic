namespace EvolvingClinic.Domain.HealthcareServices;

public class HealthcareServiceType
{
    public string Code { get; }
    private string _name;
    private TimeSpan _duration;

    private HealthcareServiceType(
        string name,
        string code,
        TimeSpan duration)
    {
        Code = code;
        _name = name;
        _duration = duration;
    }

    public static HealthcareServiceType Create(
        string name,
        string code,
        TimeSpan duration,
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
            duration);
    }

    public Snapshot CreateSnapshot()
    {
        return new Snapshot(
            Code,
            _name,
            _duration);
    }

    public record Snapshot(
        string Code,
        string Name,
        TimeSpan Duration);
}