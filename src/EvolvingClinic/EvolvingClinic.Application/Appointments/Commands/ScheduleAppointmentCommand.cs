using EvolvingClinic.Application.Common;
using EvolvingClinic.Application.HealthcareServices;
using EvolvingClinic.Domain.Appointments;

namespace EvolvingClinic.Application.Appointments.Commands;

public record ScheduleAppointmentCommand(
    DateOnly Date,
    Guid PatientId,
    string HealthcareServiceTypeCode,
    TimeOnly StartTime
) : ICommand<Guid>;

public class ScheduleAppointmentCommandHandler(
    IDailyAppointmentScheduleRepository repository,
    IHealthcareServiceTypeRepository healthcareServiceTypeRepository)
    : ICommandHandler<ScheduleAppointmentCommand, Guid>
{
    public async Task<Guid> Handle(ScheduleAppointmentCommand command)
    {
        var serviceType = await healthcareServiceTypeRepository.GetOptional(command.HealthcareServiceTypeCode);
        if (serviceType == null)
        {
            throw new ArgumentException($"Healthcare service type '{command.HealthcareServiceTypeCode}' not found");
        }
        
        var snapshot = serviceType.CreateSnapshot();
        var endTime = command.StartTime.Add(snapshot.Duration);

        var schedule = await repository.GetOptional(command.Date)
                       ?? new DailyAppointmentSchedule(command.Date);

        var appointment = schedule.ScheduleAppointment(
            command.PatientId,
            command.HealthcareServiceTypeCode,
            command.StartTime,
            endTime);

        await repository.Save(schedule);

        return appointment.Id;
    }
}