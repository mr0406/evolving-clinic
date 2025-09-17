using EvolvingClinic.Domain.Shared;

namespace EvolvingClinic.Domain.Appointments;

public class DailyAppointmentSchedule
{
    public Key ScheduleKey { get; }
    private readonly List<ScheduledAppointment> _appointments;

    public DailyAppointmentSchedule(Key scheduleKey)
    {
        ScheduleKey = scheduleKey;
        _appointments = new List<ScheduledAppointment>();
    }

    public record Key(string DoctorCode, DateOnly Date);

    public ScheduledAppointment ScheduleAppointment(
        Guid patientId,
        string healthcareServiceTypeCode,
        TimeOnly startTime,
        TimeOnly endTime,
        Money price)
    {
        var timeSlot = new AppointmentTimeSlot(ScheduleKey.Date, startTime, endTime);

        ValidateBusinessHours(timeSlot);

        if (_appointments.Any(existing => existing.HasCollisionWith(timeSlot)))
        {
            throw new ArgumentException("Appointment time slot conflicts with existing appointment");
        }

        var appointment = new ScheduledAppointment(patientId, healthcareServiceTypeCode, timeSlot, price);
        _appointments.Add(appointment);

        return appointment;
    }

    private static void ValidateBusinessHours(AppointmentTimeSlot timeSlot)
    {
        var dayOfWeek = timeSlot.Date.DayOfWeek;
        if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
        {
            throw new ArgumentException("Appointments can only be scheduled Monday through Friday");
        }

        var businessStart = new TimeOnly(9, 0);
        var businessEnd = new TimeOnly(17, 0);

        if (timeSlot.StartTime < businessStart || timeSlot.EndTime > businessEnd)
        {
            throw new ArgumentException("Appointments can only be scheduled between 9:00 AM and 5:00 PM");
        }
    }
    
    public Snapshot CreateSnapshot()
    {
        var appointmentSnapshots = _appointments.Select(a => a.CreateSnapshot()).ToList();
        return new Snapshot(ScheduleKey.DoctorCode, ScheduleKey.Date, appointmentSnapshots);
    }

    public record Snapshot(
        string DoctorCode,
        DateOnly Date,
        IReadOnlyList<ScheduledAppointment.Snapshot> Appointments);
}