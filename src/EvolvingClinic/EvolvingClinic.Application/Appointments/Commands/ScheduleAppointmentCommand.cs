using EvolvingClinic.Application.Common;
using EvolvingClinic.Domain.Appointments;

namespace EvolvingClinic.Application.Appointments.Commands;

public record ScheduleAppointmentCommand(
    DateOnly Date,
    Guid PatientId,
    TimeOnly StartTime,
    TimeOnly EndTime
) : ICommand<Guid>;

public class ScheduleAppointmentCommandHandler(IDailyAppointmentScheduleRepository repository)
    : ICommandHandler<ScheduleAppointmentCommand, Guid>
{
    public async Task<Guid> Handle(ScheduleAppointmentCommand command)
    {
        var schedule = await repository.GetOptional(command.Date) 
                       ?? new DailyAppointmentSchedule(command.Date);
        
        var appointment = schedule.ScheduleAppointment(
            command.PatientId,
            command.StartTime,
            command.EndTime);

        await repository.Save(schedule);

        return appointment.Id;
    }
}