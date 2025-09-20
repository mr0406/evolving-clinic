using EvolvingClinic.Application.Common;
using EvolvingClinic.Application.DoctorWorkSchedules;
using EvolvingClinic.Application.HealthcareServices;
using EvolvingClinic.Domain.Appointments;
using EvolvingClinic.Domain.Shared;

namespace EvolvingClinic.Application.Appointments.Commands;

public record ScheduleAppointmentCommand(
    string DoctorCode,
    DateOnly Date,
    Guid PatientId,
    string HealthcareServiceTypeCode,
    TimeOnly StartTime
) : ICommand<Guid>;

public class ScheduleAppointmentCommandHandler(
    IDailyAppointmentScheduleRepository repository,
    IHealthcareServiceTypeRepository healthcareServiceTypeRepository,
    IDoctorWorkScheduleRepository doctorWorkScheduleRepository)
    : ICommandHandler<ScheduleAppointmentCommand, Guid>
{
    public async Task<Guid> Handle(ScheduleAppointmentCommand command)
    {
        var serviceType = await healthcareServiceTypeRepository.GetOptional(command.HealthcareServiceTypeCode);
        if (serviceType == null)
        {
            throw new ArgumentException($"Healthcare service type '{command.HealthcareServiceTypeCode}' not found");
        }

        var doctorWorkSchedule = await doctorWorkScheduleRepository.GetOptional(command.DoctorCode);
        if (doctorWorkSchedule == null)
        {
            throw new ArgumentException($"Doctor work schedule for '{command.DoctorCode}' not found");
        }

        var serviceTypeSnapshot = serviceType.CreateSnapshot();
        var endTime = command.StartTime.Add(serviceTypeSnapshot.Duration);

        var scheduleKey = new DailyAppointmentSchedule.Key(command.DoctorCode, command.Date);
        var schedule = await repository.GetOptional(scheduleKey);

        if (schedule == null)
        {
            var doctorScheduleSnapshot = doctorWorkSchedule.CreateSnapshot();
            var dayOfWeek = command.Date.DayOfWeek;

            if (!doctorScheduleSnapshot.WeeklySchedule.TryGetValue(dayOfWeek, out var workingHours))
            {
                throw new ArgumentException($"Doctor '{command.DoctorCode}' does not work on {dayOfWeek}");
            }

            schedule = DailyAppointmentSchedule.Create(scheduleKey, workingHours);
        }

        var appointment = schedule.ScheduleAppointment(
            command.PatientId,
            command.HealthcareServiceTypeCode,
            new TimeRange(command.StartTime, endTime),
            serviceTypeSnapshot.Price);

        await repository.Save(schedule);

        return appointment.Id;
    }
}