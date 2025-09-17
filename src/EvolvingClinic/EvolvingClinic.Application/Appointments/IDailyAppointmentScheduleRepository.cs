using EvolvingClinic.Domain.Appointments;

namespace EvolvingClinic.Application.Appointments;

public interface IDailyAppointmentScheduleRepository
{
    Task<DailyAppointmentSchedule?> GetOptional(DailyAppointmentSchedule.Key key);
    Task<DailyAppointmentScheduleDto> GetDto(DailyAppointmentSchedule.Key key);
    Task Save(DailyAppointmentSchedule schedule);
}