using EvolvingClinic.Application.Common;
using EvolvingClinic.Domain.Doctors;
using EvolvingClinic.Domain.Shared;

namespace EvolvingClinic.Application.Doctors.Commands;

public record RegisterDoctorCommand(
    string Code,
    RegisterDoctorCommand.PersonNameData Name
) : ICommand<string>
{
    public record PersonNameData(string FirstName, string LastName);
}

public class RegisterDoctorCommandHandler(IDoctorRepository repository)
    : ICommandHandler<RegisterDoctorCommand, string>
{
    public async Task<string> Handle(RegisterDoctorCommand command)
    {
        var name = new PersonName(command.Name.FirstName, command.Name.LastName);

        var existingCodes = await repository.GetAllCodes();

        var doctor = Doctor.Register(
            command.Code,
            name,
            existingCodes.ToList());

        await repository.Save(doctor);

        return doctor.Code;
    }
}