namespace EvolvingClinic.Domain.Appointments;

public class DailyAppointmentSchedule
{
    public DateOnly Date { get; }
    private readonly List<ScheduledAppointment> _appointments;

    public DailyAppointmentSchedule(DateOnly date)
    {
        Date = date;
        _appointments = new List<ScheduledAppointment>();
    }

    public ScheduledAppointment ScheduleAppointment(
        Guid patientId,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        var timeSlot = new AppointmentTimeSlot(Date, startTime, endTime);

        ValidateBusinessHours(timeSlot);

        if (_appointments.Any(existing => existing.HasCollisionWith(timeSlot)))
        {
            throw new ArgumentException("Appointment time slot conflicts with existing appointment");
        }
        
        var appointment = new ScheduledAppointment(patientId, timeSlot);
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
        return new Snapshot(Date, appointmentSnapshots);
    }

    public record Snapshot(
        DateOnly Date,
        IReadOnlyList<ScheduledAppointment.Snapshot> Appointments);
}