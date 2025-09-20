using EvolvingClinic.Domain.Shared;

namespace EvolvingClinic.Application.DoctorWorkSchedules;

public record DoctorWorkScheduleDto(
    string DoctorCode,
    Dictionary<DayOfWeek, TimeRange> WeeklySchedule);