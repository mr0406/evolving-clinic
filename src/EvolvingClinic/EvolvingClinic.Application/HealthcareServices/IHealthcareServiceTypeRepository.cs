using EvolvingClinic.Domain.HealthcareServices;

namespace EvolvingClinic.Application.HealthcareServices;

public interface IHealthcareServiceTypeRepository
{
    Task<HealthcareServiceType?> GetOptional(string code);
    Task<HealthcareServiceTypeDto> GetDto(string code);
    Task<IReadOnlyList<HealthcareServiceTypeDto>> GetAllDtos();
    Task<IReadOnlyList<string>> GetAllNames();
    Task<IReadOnlyList<string>> GetAllCodes();
    Task Save(HealthcareServiceType healthcareServiceType);
}