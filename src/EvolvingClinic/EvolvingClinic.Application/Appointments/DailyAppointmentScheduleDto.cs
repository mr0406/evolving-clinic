namespace EvolvingClinic.Application.Appointments;

public record DailyAppointmentScheduleDto(
    DateOnly Date,
    IReadOnlyList<ScheduledAppointmentDto> Appointments);

public record ScheduledAppointmentDto(
    Guid Id,
    string PatientName,
    DateTime StartTime,
    DateTime EndTime);