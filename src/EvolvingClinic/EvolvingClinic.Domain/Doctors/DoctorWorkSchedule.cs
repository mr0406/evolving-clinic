using EvolvingClinic.Domain.Shared;

namespace EvolvingClinic.Domain.Doctors;

public class DoctorWorkSchedule
{
    public string DoctorCode { get; }
    private readonly Dictionary<DayOfWeek, TimeRange> _weeklySchedule;

    private DoctorWorkSchedule(string doctorCode, Dictionary<DayOfWeek, TimeRange> weeklySchedule)
    {
        DoctorCode = doctorCode;
        _weeklySchedule = weeklySchedule;
    }

    public static DoctorWorkSchedule Create(string doctorCode, List<WorkingDay> workingDays)
    {
        if (string.IsNullOrWhiteSpace(doctorCode))
        {
            throw new ArgumentException("Doctor code is required");
        }

        if (workingDays.Count == 0)
        {
            throw new ArgumentException("At least one working day is required");
        }

        var duplicatedDays = workingDays
            .GroupBy(wd => wd.Day)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
        
        if (duplicatedDays.Any())
        {
            throw new ArgumentException($"Duplicate days found: {string.Join(", ", duplicatedDays)}");
        }

        var weeklySchedule = workingDays.ToDictionary(wd => wd.Day, wd => wd.Hours);
        
        return new DoctorWorkSchedule(doctorCode, weeklySchedule);
    }

    public Snapshot CreateSnapshot()
    {
        return new Snapshot(
            DoctorCode,
            new Dictionary<DayOfWeek, TimeRange>(_weeklySchedule));
    }

    public record WorkingDay(DayOfWeek Day, TimeRange Hours);

    public record Snapshot(
        string DoctorCode,
        Dictionary<DayOfWeek, TimeRange> WeeklySchedule);
}