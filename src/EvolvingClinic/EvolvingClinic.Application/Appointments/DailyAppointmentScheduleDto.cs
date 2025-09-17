namespace EvolvingClinic.Application.Appointments;

public record DailyAppointmentScheduleDto(
    string DoctorCode,
    DateOnly Date,
    IReadOnlyList<ScheduledAppointmentDto> Appointments);

public record ScheduledAppointmentDto(
    Guid Id,
    Guid PatientId,
    string HealthcareServiceTypeCode,
    DateTime StartTime,
    DateTime EndTime,
    decimal Price);