namespace EvolvingClinic.Domain.Appointments;

public class ScheduledAppointment 
{
    public Guid Id { get; }
    private Guid _patientId;
    private AppointmentTimeSlot _timeSlot;
    
    internal ScheduledAppointment(
        Guid patientId, 
        AppointmentTimeSlot timeSlot)
    {
        Id = Guid.NewGuid();
        _patientId = patientId;
        _timeSlot = timeSlot;
    }

    public bool HasCollisionWith(AppointmentTimeSlot otherTimeSlot)
    {
        return _timeSlot.OverlapsWith(otherTimeSlot);
    }

    public Snapshot CreateSnapshot()
    {
        return new Snapshot(Id, _patientId, _timeSlot.StartDateTime, _timeSlot.EndDateTime);
    }
    
    public record Snapshot(
        Guid Id,
        Guid PatientId,
        DateTime StartTime,
        DateTime EndTime);
}