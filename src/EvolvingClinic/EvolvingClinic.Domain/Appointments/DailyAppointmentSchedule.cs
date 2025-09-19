using EvolvingClinic.Domain.Shared;

namespace EvolvingClinic.Domain.Appointments;

public class DailyAppointmentSchedule
{
    public Key ScheduleKey { get; }
    private readonly TimeRange _workingHours;
    private readonly List<ScheduledAppointment> _appointments;

    private DailyAppointmentSchedule(Key scheduleKey, TimeRange workingHours)
    {
        ScheduleKey = scheduleKey;
        _workingHours = workingHours;
        _appointments = new List<ScheduledAppointment>();
    }

    public static DailyAppointmentSchedule Create(Key scheduleKey, TimeRange workingHours)
    {
        return new DailyAppointmentSchedule(scheduleKey, workingHours);
    }

    public record Key(string DoctorCode, DateOnly Date);

    public ScheduledAppointment ScheduleAppointment(
        Guid patientId,
        string healthcareServiceTypeCode,
        TimeRange appointmentTime,
        Money price)
    {
        var timeSlot = new AppointmentTimeSlot(ScheduleKey.Date, appointmentTime.Start, appointmentTime.End);

        ValidateWorkingHours(timeSlot);

        if (_appointments.Any(existing => existing.HasCollisionWith(timeSlot)))
        {
            throw new ArgumentException("Appointment time slot conflicts with existing appointment");
        }

        var appointment = new ScheduledAppointment(patientId, healthcareServiceTypeCode, timeSlot, price);
        _appointments.Add(appointment);

        return appointment;
    }

    private void ValidateWorkingHours(AppointmentTimeSlot timeSlot)
    {
        if (timeSlot.StartTime < _workingHours.Start || timeSlot.EndTime > _workingHours.End)
        {
            throw new ArgumentException($"Appointments can only be scheduled between {_workingHours.Start:HH:mm} and {_workingHours.End:HH:mm}");
        }
    }
    
    public Snapshot CreateSnapshot()
    {
        var appointmentSnapshots = _appointments.Select(a => a.CreateSnapshot()).ToList();
        return new Snapshot(ScheduleKey.DoctorCode, ScheduleKey.Date, _workingHours, appointmentSnapshots);
    }

    public record Snapshot(
        string DoctorCode,
        DateOnly Date,
        TimeRange WorkingHours,
        IReadOnlyList<ScheduledAppointment.Snapshot> Appointments);
}