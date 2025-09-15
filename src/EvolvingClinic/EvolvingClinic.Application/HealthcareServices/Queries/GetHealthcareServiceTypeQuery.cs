using EvolvingClinic.Application.Common;

namespace EvolvingClinic.Application.HealthcareServices.Queries;

public record GetHealthcareServiceTypeQuery(string Code) : IQuery<HealthcareServiceTypeDto>;

public class GetHealthcareServiceTypeQueryHandler(IHealthcareServiceTypeRepository repository)
    : IQueryHandler<GetHealthcareServiceTypeQuery, HealthcareServiceTypeDto>
{
    public async Task<HealthcareServiceTypeDto> Handle(GetHealthcareServiceTypeQuery query)
    {
        return await repository.GetDto(query.Code);
    }
}