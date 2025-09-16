namespace EvolvingClinic.Application.HealthcareServices;

public record HealthcareServiceTypeDto(
    string Code,
    string Name,
    TimeSpan Duration,
    decimal Price,
    IReadOnlyList<PriceHistoryEntryData> PriceHistory);

public record PriceHistoryEntryData(
    decimal Price,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo);