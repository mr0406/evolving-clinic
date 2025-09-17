using EvolvingClinic.Domain.Appointments;

namespace EvolvingClinic.Application.Appointments;

public class InMemoryDailyAppointmentScheduleRepository : IDailyAppointmentScheduleRepository
{
    private static List<DailyAppointmentSchedule> _schedules = new();

    public Task<DailyAppointmentSchedule?> GetOptional(DailyAppointmentSchedule.Key key)
    {
        var schedule = _schedules.SingleOrDefault(s => s.ScheduleKey == key);

        return Task.FromResult(schedule);
    }

    public Task<DailyAppointmentScheduleDto> GetDto(DailyAppointmentSchedule.Key key)
    {
        var schedule = _schedules.SingleOrDefault(s => s.ScheduleKey == key);

        if (schedule is null)
        {
            return Task.FromResult(new DailyAppointmentScheduleDto(key.DoctorCode, key.Date, []));
        }

        var snapshot = schedule.CreateSnapshot();

        var appointmentDtos = snapshot.Appointments.Select(a => new ScheduledAppointmentDto(
            a.Id,
            a.PatientId,
            a.HealthcareServiceTypeCode,
            a.StartTime,
            a.EndTime,
            a.Price.Value)).ToList();

        return Task.FromResult(new DailyAppointmentScheduleDto(snapshot.DoctorCode, snapshot.Date, appointmentDtos));
    }

    public Task Save(DailyAppointmentSchedule schedule)
    {
        var existingIndex = _schedules.FindIndex(s => s.ScheduleKey == schedule.ScheduleKey);

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