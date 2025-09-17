using EvolvingClinic.Application.Common;

namespace EvolvingClinic.Application.Doctors.Queries;

public record GetAllDoctorsQuery() : IQuery<IReadOnlyList<DoctorDto>>;

public class GetAllDoctorsQueryHandler(IDoctorRepository repository)
    : IQueryHandler<GetAllDoctorsQuery, IReadOnlyList<DoctorDto>>
{
    public async Task<IReadOnlyList<DoctorDto>> Handle(GetAllDoctorsQuery query)
    {
        return await repository.GetAllDtos();
    }
}