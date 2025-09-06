namespace EvolvingClinic.Application.Patients;

public record PatientDto(
    Guid Id,
    PatientDto.PersonNameData Name,
    DateOnly DateOfBirth,
    PatientDto.PhoneNumberData PhoneNumber,
    PatientDto.AddressData Address)
{
    public record PersonNameData(string FirstName, string LastName);
    public record PhoneNumberData(string CountryCode, string Number);
    public record AddressData(string Street, string HouseNumber, string? Apartment, string PostalCode, string City);
}