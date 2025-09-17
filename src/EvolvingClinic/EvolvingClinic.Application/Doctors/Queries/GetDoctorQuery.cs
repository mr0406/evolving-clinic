using EvolvingClinic.Application.Common;

namespace EvolvingClinic.Application.Doctors.Queries;

public record GetDoctorQuery(string Code) : IQuery<DoctorDto>;

public class GetDoctorQueryHandler(IDoctorRepository repository)
    : IQueryHandler<GetDoctorQuery, DoctorDto>
{
    public async Task<DoctorDto> Handle(GetDoctorQuery query)
    {
        return await repository.GetDto(query.Code);
    }
}