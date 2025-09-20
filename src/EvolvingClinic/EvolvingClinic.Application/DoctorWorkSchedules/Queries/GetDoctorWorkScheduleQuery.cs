using EvolvingClinic.Application.Common;

namespace EvolvingClinic.Application.DoctorWorkSchedules.Queries;

public record GetDoctorWorkScheduleQuery(string DoctorCode) : IQuery<DoctorWorkScheduleDto?>;

public class GetDoctorWorkScheduleQueryHandler(IDoctorWorkScheduleRepository repository)
    : IQueryHandler<GetDoctorWorkScheduleQuery, DoctorWorkScheduleDto?>
{
    public async Task<DoctorWorkScheduleDto?> Handle(GetDoctorWorkScheduleQuery query)
    {
        return await repository.GetDtoOptional(query.DoctorCode);
    }
}