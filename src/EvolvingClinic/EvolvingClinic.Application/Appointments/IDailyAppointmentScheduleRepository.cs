using EvolvingClinic.Domain;
using EvolvingClinic.Domain.Appointments;

namespace EvolvingClinic.Application.Appointments;

public interface IDailyAppointmentScheduleRepository
{
    Task<DailyAppointmentSchedule> Get(DateOnly date);
    Task<DailyAppointmentScheduleDto> GetDto(DateOnly date);
    Task Save(DailyAppointmentSchedule schedule);
}