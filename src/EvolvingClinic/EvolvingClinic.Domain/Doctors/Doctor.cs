using EvolvingClinic.Domain.Shared;

namespace EvolvingClinic.Domain.Doctors;

public class Doctor
{
    public string Code { get; }
    private PersonName _name;

    private Doctor(
        string code,
        PersonName name)
    {
        Code = code;
        _name = name;
    }

    public static Doctor Register(
        string code,
        PersonName name,
        List<string> existingCodes)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Doctor code is required");
        }

        code = code.Trim().ToUpperInvariant();

        if (existingCodes.Any(c => string.Equals(c, code, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException($"A doctor with the code '{code}' already exists");
        }

        return new Doctor(
            code,
            name);
    }

    public Snapshot CreateSnapshot()
    {
        return new Snapshot(
            Code,
            _name);
    }

    public record Snapshot(
        string Code,
        PersonName Name);
}