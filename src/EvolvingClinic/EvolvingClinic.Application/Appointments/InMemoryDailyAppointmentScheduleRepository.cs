using EvolvingClinic.Domain.Appointments;

namespace EvolvingClinic.Application.Appointments;

public class InMemoryDailyAppointmentScheduleRepository : IDailyAppointmentScheduleRepository
{
    private static List<DailyAppointmentSchedule> _schedules = new();

    public Task<DailyAppointmentSchedule?> GetOptional(DateOnly date)
    {
        var schedule = _schedules.SingleOrDefault(s => s.Date == date);

        return Task.FromResult(schedule);
    }

    public Task<DailyAppointmentScheduleDto> GetDto(DateOnly date)
    {
        var schedule = _schedules.FirstOrDefault(s => s.Date == date);
        
        if (schedule is null)
        {
            schedule = new DailyAppointmentSchedule(date);
        }

        var snapshot = schedule.CreateSnapshot();
        
        var appointmentDtos = snapshot.Appointments.Select(a => new ScheduledAppointmentDto(
            a.Id,
            a.PatientId,
            a.HealthcareServiceTypeCode,
            a.StartTime,
            a.EndTime,
            a.Price.Value)).ToList();

        return Task.FromResult(new DailyAppointmentScheduleDto(snapshot.Date, appointmentDtos));
    }

    public Task Save(DailyAppointmentSchedule schedule)
    {
        var existingIndex = _schedules.FindIndex(s => s.Date == schedule.Date);
        
        if (existingIndex >= 0)
        {
            _schedules[existingIndex] = schedule;
        }
        else
        {
            _schedules.Add(schedule);
        }

        return Task.CompletedTask;
    }

    public static void Clear()
    {
        _schedules = new();
    }
}