using EvolvingClinic.Application.Common;
using EvolvingClinic.Domain.Doctors;
using EvolvingClinic.Domain.Shared;

namespace EvolvingClinic.Application.Doctors.Commands;

public record RegisterDoctorCommand(
    string Code,
    string FirstName,
    string LastName
) : ICommand;

public class RegisterDoctorCommandHandler(IDoctorRepository repository)
    : ICommandHandler<RegisterDoctorCommand>
{
    public async Task Handle(RegisterDoctorCommand command)
    {
        var name = new PersonName(command.FirstName, command.LastName);

        var existingCodes = await repository.GetAllCodes();

        var doctor = Doctor.Register(
            command.Code,
            name,
            existingCodes.ToList());

        await repository.Save(doctor);
    }
}