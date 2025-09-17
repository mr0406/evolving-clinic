using EvolvingClinic.Application.Common;
using EvolvingClinic.Domain.Appointments;

namespace EvolvingClinic.Application.Appointments.Queries;

public record GetDailyAppointmentScheduleQuery(string DoctorCode, DateOnly Date) : IQuery<DailyAppointmentScheduleDto>;

public class GetDailyAppointmentScheduleQueryHandler(IDailyAppointmentScheduleRepository repository)
    : IQueryHandler<GetDailyAppointmentScheduleQuery, DailyAppointmentScheduleDto>
{
    public async Task<DailyAppointmentScheduleDto> Handle(GetDailyAppointmentScheduleQuery query)
    {
        var key = new DailyAppointmentSchedule.Key(query.DoctorCode, query.Date);
        return await repository.GetDto(key);
    }
}