using EvolvingClinic.Domain.DoctorWorkSchedules;

namespace EvolvingClinic.Application.DoctorWorkSchedules;

public class InMemoryDoctorWorkScheduleRepository : IDoctorWorkScheduleRepository
{
    private readonly Dictionary<string, DoctorWorkSchedule.Snapshot> _schedules = new();

    public Task<DoctorWorkSchedule?> GetOptional(string doctorCode)
    {
        if (!_schedules.TryGetValue(doctorCode, out var snapshot))
        {
            return Task.FromResult<DoctorWorkSchedule?>(null);
        }

        var workingDays = snapshot.WeeklySchedule
            .Select(kvp => new DoctorWorkSchedule.WorkingDay(kvp.Key, kvp.Value))
            .ToList();

        var schedule = DoctorWorkSchedule.Create(snapshot.DoctorCode, workingDays);
        return Task.FromResult<DoctorWorkSchedule?>(schedule);
    }

    public Task<DoctorWorkScheduleDto?> GetDtoOptional(string doctorCode)
    {
        if (!_schedules.TryGetValue(doctorCode, out var snapshot))
        {
            return Task.FromResult<DoctorWorkScheduleDto?>(null);
        }

        var dto = new DoctorWorkScheduleDto(snapshot.DoctorCode, snapshot.WeeklySchedule);
        return Task.FromResult<DoctorWorkScheduleDto?>(dto);
    }

    public Task Save(DoctorWorkSchedule schedule)
    {
        var snapshot = schedule.CreateSnapshot();
        _schedules[schedule.DoctorCode] = snapshot;
        return Task.CompletedTask;
    }
}