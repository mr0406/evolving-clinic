namespace EvolvingClinic.Application.Appointments;

public record DailyAppointmentScheduleDto(
    DateOnly Date,
    IReadOnlyList<ScheduledAppointmentDto> Appointments);

public record ScheduledAppointmentDto(
    Guid Id,
    Guid PatientId,
    string HealthcareServiceTypeCode,
    DateTime StartTime,
    DateTime EndTime);