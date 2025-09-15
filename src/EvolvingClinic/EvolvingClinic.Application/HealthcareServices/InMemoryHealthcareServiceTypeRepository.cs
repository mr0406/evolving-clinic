using EvolvingClinic.Domain.HealthcareServices;

namespace EvolvingClinic.Application.HealthcareServices;

public class InMemoryHealthcareServiceTypeRepository : IHealthcareServiceTypeRepository
{
    private static List<HealthcareServiceType> _healthcareServiceTypes = new();

    public Task<HealthcareServiceType?> GetOptional(Guid id)
    {
        var serviceType = _healthcareServiceTypes.SingleOrDefault(s => s.Id == id);

        return Task.FromResult(serviceType);
    }

    public Task<HealthcareServiceTypeDto> GetDto(Guid id)
    {
        var serviceType = _healthcareServiceTypes.SingleOrDefault(s => s.Id == id);

        if (serviceType is null)
        {
            throw new InvalidOperationException($"Healthcare service type with id {id} not found");
        }

        var snapshot = serviceType.CreateSnapshot();

        return Task.FromResult(new HealthcareServiceTypeDto(
            snapshot.Id,
            snapshot.Name,
            snapshot.Code,
            snapshot.Duration));
    }

    public Task<IReadOnlyList<HealthcareServiceTypeDto>> GetAllDtos()
    {
        var dtos = _healthcareServiceTypes.Select(s =>
        {
            var snapshot = s.CreateSnapshot();
            return new HealthcareServiceTypeDto(
                snapshot.Id,
                snapshot.Name,
                snapshot.Code,
                snapshot.Duration
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
            .Select(s => s.CreateSnapshot().Code)
            .ToList();

        return Task.FromResult<IReadOnlyList<string>>(codes);
    }

    public Task Save(HealthcareServiceType healthcareServiceType)
    {
        var existingIndex = _healthcareServiceTypes.FindIndex(s => s.Id == healthcareServiceType.Id);

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