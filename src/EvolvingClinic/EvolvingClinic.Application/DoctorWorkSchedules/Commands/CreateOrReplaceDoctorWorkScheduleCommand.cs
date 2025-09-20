using EvolvingClinic.Application.Common;
using EvolvingClinic.Application.Doctors;
using EvolvingClinic.Domain.DoctorWorkSchedules;
using EvolvingClinic.Domain.Shared;

namespace EvolvingClinic.Application.DoctorWorkSchedules.Commands;

public record CreateOrReplaceDoctorWorkScheduleCommand(
    string DoctorCode,
    List<CreateOrReplaceDoctorWorkScheduleCommand.WorkingDayData> WorkingDays
) : ICommand
{
    public record WorkingDayData(DayOfWeek Day, TimeRange Hours);
}

public class CreateOrReplaceDoctorWorkScheduleCommandHandler(
    IDoctorWorkScheduleRepository repository,
    IDoctorRepository doctorRepository)
    : ICommandHandler<CreateOrReplaceDoctorWorkScheduleCommand>
{
    public async Task Handle(CreateOrReplaceDoctorWorkScheduleCommand command)
    {
        var doctor = await doctorRepository.GetOptional(command.DoctorCode);
        if (doctor == null)
        {
            throw new ArgumentException($"Doctor with code '{command.DoctorCode}' not found");
        }

        var workingDays = command.WorkingDays
            .Select(wd => new DoctorWorkSchedule.WorkingDay(wd.Day, wd.Hours))
            .ToList();

        var schedule = DoctorWorkSchedule.Create(command.DoctorCode, workingDays);

        await repository.Save(schedule);
    }
}