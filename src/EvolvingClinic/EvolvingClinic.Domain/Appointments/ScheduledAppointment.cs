namespace EvolvingClinic.Domain.Appointments;

public class ScheduledAppointment 
{
    public Guid Id { get; }
    private string _patientName;
    private AppointmentTimeSlot _timeSlot;
    
    internal ScheduledAppointment(
        string patientName, 
        AppointmentTimeSlot timeSlot)
    {
        Id = Guid.NewGuid();
        _patientName = patientName;
        _timeSlot = timeSlot;
    }

    public bool HasCollisionWith(AppointmentTimeSlot otherTimeSlot)
    {
        return _timeSlot.OverlapsWith(otherTimeSlot);
    }

    public Snapshot CreateSnapshot()
    {
        return new Snapshot(Id, _patientName, _timeSlot.StartDateTime, _timeSlot.EndDateTime);
    }
    
    public record Snapshot(
        Guid Id,
        string PatientName,
        DateTime StartTime,
        DateTime EndTime);
}