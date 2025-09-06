using EvolvingClinic.Application.Common;
using EvolvingClinic.Domain.Patients;
using EvolvingClinic.Domain.Shared;

namespace EvolvingClinic.Application.Patients.Commands;

public record RegisterPatientCommand(
    RegisterPatientCommand.PersonNameData Name,
    DateOnly DateOfBirth,
    RegisterPatientCommand.PhoneNumberData Phone,
    RegisterPatientCommand.AddressData Address
) : ICommand<Guid>
{
    public record PersonNameData(string FirstName, string LastName);
    public record PhoneNumberData(string CountryCode, string Number);
    public record AddressData(string Street, string HouseNumber, string? Apartment, string PostalCode, string City);
};

public class RegisterPatientCommandHandler(IPatientRepository repository)
    : ICommandHandler<RegisterPatientCommand, Guid>
{
    public async Task<Guid> Handle(RegisterPatientCommand command)
    {
        var name = new PersonName(command.Name.FirstName, command.Name.LastName);
        var phoneNumber = new PhoneNumber(command.Phone.CountryCode, command.Phone.Number);
        var address = new Address(
            command.Address.Street, 
            command.Address.HouseNumber, 
            command.Address.Apartment, 
            command.Address.PostalCode, 
            command.Address.City);

        var patient = Patient.Register(
            name,
            command.DateOfBirth,
            phoneNumber,
            address);

        await repository.Save(patient);

        return patient.Id;
    }
}