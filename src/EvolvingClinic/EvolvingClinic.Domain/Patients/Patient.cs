using EvolvingClinic.Domain.Shared;

namespace EvolvingClinic.Domain.Patients;

public class Patient
{
    public Guid Id { get; }
    private PersonName _name;
    private DateOnly _dateOfBirth;
    private PhoneNumber _phoneNumber;
    private Address _address;

    private Patient(
        PersonName name,
        DateOnly dateOfBirth,
        PhoneNumber phoneNumber,
        Address address)
    {
        Id = Guid.NewGuid();
        _name = name;
        _dateOfBirth = dateOfBirth;
        _phoneNumber = phoneNumber;
        _address = address;
    }

    public static Patient Register(
        PersonName name,
        DateOnly dateOfBirth,
        PhoneNumber phoneNumber,
        Address address)
    {
        return new Patient(
            name,
            dateOfBirth,
            phoneNumber,
            address);
    }

    public Snapshot CreateSnapshot()
    {
        return new Snapshot(
            Id,
            _name,
            _dateOfBirth,
            _phoneNumber,
            _address);
    }

    public record Snapshot(
        Guid Id,
        PersonName Name,
        DateOnly DateOfBirth,
        PhoneNumber PhoneNumber,
        Address Address);
}