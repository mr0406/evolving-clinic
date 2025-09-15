using EvolvingClinic.Domain.HealthcareServices;

namespace EvolvingClinic.Application.HealthcareServices;

public class InMemoryHealthcareServiceTypeRepository : IHealthcareServiceTypeRepository
{
    private static List<HealthcareServiceType> _healthcareServiceTypes = new();

    public Task<HealthcareServiceType?> GetOptional(string code)
    {
        var serviceType = _healthcareServiceTypes.SingleOrDefault(s => s.Code == code);

        return Task.FromResult(serviceType);
    }

    public Task<HealthcareServiceTypeDto> GetDto(string code)
    {
        var serviceType = _healthcareServiceTypes.SingleOrDefault(s => s.Code == code);

        if (serviceType is null)
        {
            throw new InvalidOperationException($"Healthcare service type with code {code} not found");
        }

        var snapshot = serviceType.CreateSnapshot();

        return Task.FromResult(new HealthcareServiceTypeDto(
            snapshot.Code,
            snapshot.Name,
            snapshot.Duration,
            snapshot.Price.Value));
    }

    public Task<IReadOnlyList<HealthcareServiceTypeDto>> GetAllDtos()
    {
        var dtos = _healthcareServiceTypes.Select(s =>
        {
            var snapshot = s.CreateSnapshot();
            return new HealthcareServiceTypeDto(
                snapshot.Code,
                snapshot.Name,
                snapshot.Duration,
                snapshot.Price.Value
            );
        }).ToList();

        return Task.FromResult<IReadOnlyList<HealthcareServiceTypeDto>>(dtos);
    }

    public Task<IReadOnlyList<string>> GetAllNames()
    {
        var names = _healthcareServiceTypes
            .Select(s => s.CreateSnapshot().Name)
            .ToList();

        return Task.FromResult<IReadOnlyList<string>>(names);
    }

    public Task<IReadOnlyList<string>> GetAllCodes()
    {
        var codes = _healthcareServiceTypes
            .Select(s => s.Code)
            .ToList();

        return Task.FromResult<IReadOnlyList<string>>(codes);
    }

    public Task Save(HealthcareServiceType healthcareServiceType)
    {
        var existingIndex = _healthcareServiceTypes.FindIndex(s => s.Code == healthcareServiceType.Code);

        if (existingIndex >= 0)
        {
            _healthcareServiceTypes[existingIndex] = healthcareServiceType;
        }
        else
        {
            _healthcareServiceTypes.Add(healthcareServiceType);
        }

        return Task.CompletedTask;
    }

    public static void Clear()
    {
        _healthcareServiceTypes = new();
    }
}