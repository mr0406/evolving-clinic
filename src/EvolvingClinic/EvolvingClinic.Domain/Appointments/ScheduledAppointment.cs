namespace EvolvingClinic.Domain.Appointments;

public class ScheduledAppointment
{
    public Guid Id { get; }
    private Guid _patientId;
    private string _healthcareServiceTypeCode;
    private AppointmentTimeSlot _timeSlot;

    internal ScheduledAppointment(
        Guid patientId,
        string healthcareServiceTypeCode,
        AppointmentTimeSlot timeSlot)
    {
        Id = Guid.NewGuid();
        _patientId = patientId;
        _healthcareServiceTypeCode = healthcareServiceTypeCode;
        _timeSlot = timeSlot;
    }

    public bool HasCollisionWith(AppointmentTimeSlot otherTimeSlot)
    {
        return _timeSlot.OverlapsWith(otherTimeSlot);
    }

    public Snapshot CreateSnapshot()
    {
        return new Snapshot(Id, _patientId, _healthcareServiceTypeCode, _timeSlot.StartDateTime, _timeSlot.EndDateTime);
    }

    public record Snapshot(
        Guid Id,
        Guid PatientId,
        string HealthcareServiceTypeCode,
        DateTime StartTime,
        DateTime EndTime);
}