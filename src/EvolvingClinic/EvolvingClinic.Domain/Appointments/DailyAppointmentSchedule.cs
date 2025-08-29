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
        string patientName,
        AppointmentTimeSlot timeSlot)
    {
        if (timeSlot.Date != Date)
        {
            throw new ArgumentException("Appointment must be scheduled for the same date as the schedule");
        }

        if (_appointments.Any(existing => existing.HasCollisionWith(timeSlot)))
        {
            throw new ArgumentException("Appointment time slot conflicts with existing appointment");
        }
        
        var appointment = new ScheduledAppointment(patientName, timeSlot);
        _appointments.Add(appointment);
        
        return appointment;
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