using EvolvingClinic.Domain.Shared;

namespace EvolvingClinic.Domain.Appointments;

public class ScheduledAppointment
{
    public Guid Id { get; }
    private Guid _patientId;
    private string _healthcareServiceTypeCode;
    private AppointmentTimeSlot _timeSlot;
    private Money _price;

    internal ScheduledAppointment(
        Guid patientId,
        string healthcareServiceTypeCode,
        AppointmentTimeSlot timeSlot,
        Money price)
    {
        if (price.Value < 0)
        {
            throw new ArgumentException("Appointment price must be 0 or greater");
        }

        Id = Guid.NewGuid();
        _patientId = patientId;
        _healthcareServiceTypeCode = healthcareServiceTypeCode;
        _timeSlot = timeSlot;
        _price = price;
    }

    public bool HasCollisionWith(AppointmentTimeSlot otherTimeSlot)
    {
        return _timeSlot.OverlapsWith(otherTimeSlot);
    }

    public Snapshot CreateSnapshot()
    {
        return new Snapshot(Id, _patientId, _healthcareServiceTypeCode, _timeSlot.StartDateTime, _timeSlot.EndDateTime, _price);
    }

    public record Snapshot(
        Guid Id,
        Guid PatientId,
        string HealthcareServiceTypeCode,
        DateTime StartTime,
        DateTime EndTime,
        Money Price);
}