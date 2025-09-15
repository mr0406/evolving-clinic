namespace EvolvingClinic.Application.HealthcareServices;

public record HealthcareServiceTypeDto(
    Guid Id,
    string Name,
    string Code,
    TimeSpan Duration);