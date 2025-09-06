using EvolvingClinic.Application.Common;

namespace EvolvingClinic.Application.Patients.Queries;

public record GetPatientQuery(Guid Id) : IQuery<PatientDto>;

public class GetPatientQueryHandler(IPatientRepository repository)
    : IQueryHandler<GetPatientQuery, PatientDto>
{
    public async Task<PatientDto> Handle(GetPatientQuery query)
    {
        return await repository.GetDto(query.Id);
    }
}