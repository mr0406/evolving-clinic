using EvolvingClinic.Application.Common;

namespace EvolvingClinic.Application.HealthcareServices.Queries;

public record GetAllHealthcareServiceTypesQuery() : IQuery<IReadOnlyList<HealthcareServiceTypeDto>>;

public class GetAllHealthcareServiceTypesQueryHandler(IHealthcareServiceTypeRepository repository)
    : IQueryHandler<GetAllHealthcareServiceTypesQuery, IReadOnlyList<HealthcareServiceTypeDto>>
{
    public async Task<IReadOnlyList<HealthcareServiceTypeDto>> Handle(GetAllHealthcareServiceTypesQuery query)
    {
        return await repository.GetAllDtos();
    }
}