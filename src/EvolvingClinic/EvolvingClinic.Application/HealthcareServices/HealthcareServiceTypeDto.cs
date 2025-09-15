namespace EvolvingClinic.Application.HealthcareServices;

public record HealthcareServiceTypeDto(
    string Code,
    string Name,
    TimeSpan Duration,
    decimal Price);