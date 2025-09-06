using EvolvingClinic.Application.Common;

namespace EvolvingClinic.Application.Patients.Queries;

public record GetAllPatientsQuery : IQuery<IReadOnlyList<PatientDto>>;

public sealed class GetAllPatientsQueryHandler(IPatientRepository patientRepository)
    : IQueryHandler<GetAllPatientsQuery, IReadOnlyList<PatientDto>>
{
    public async Task<IReadOnlyList<PatientDto>> Handle(GetAllPatientsQuery query)
    {
        return await patientRepository.GetAllDtos();
    }
}