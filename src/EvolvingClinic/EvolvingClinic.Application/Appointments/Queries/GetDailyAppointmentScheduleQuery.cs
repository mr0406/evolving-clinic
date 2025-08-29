using EvolvingClinic.Application.Common;

namespace EvolvingClinic.Application.Appointments.Queries;

public record GetDailyAppointmentScheduleQuery(DateOnly Date) : IQuery<DailyAppointmentScheduleDto>;

public class GetDailyAppointmentScheduleQueryHandler(IDailyAppointmentScheduleRepository repository)
    : IQueryHandler<GetDailyAppointmentScheduleQuery, DailyAppointmentScheduleDto>
{
    public async Task<DailyAppointmentScheduleDto> Handle(GetDailyAppointmentScheduleQuery query)
    {
        return await repository.GetDto(query.Date);
    }
}