using EvolvingClinic.Domain.HealthcareServices;

namespace EvolvingClinic.Application.HealthcareServices;

public interface IHealthcareServiceTypeRepository
{
    Task<HealthcareServiceType?> GetOptional(Guid id);
    Task<HealthcareServiceTypeDto> GetDto(Guid id);
    Task<IReadOnlyList<HealthcareServiceTypeDto>> GetAllDtos();
    Task<IReadOnlyList<string>> GetAllNames();
    Task<IReadOnlyList<string>> GetAllCodes();
    Task Save(HealthcareServiceType healthcareServiceType);
}