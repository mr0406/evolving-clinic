using EvolvingClinic.Application.Common;
using EvolvingClinic.Application.Patients.Commands;
using EvolvingClinic.Application.Patients.Queries;
using Reqnroll;
using Shouldly;

namespace EvolvingClinic.BusinessTests.StepDefinitions;

[Binding]
public sealed class RegisterPatientStepDefinitions
{
    private readonly Dispatcher _dispatcher = new();
    private Guid? _scenarioPatientId;
    private RegisterPatientData? _scenarioRegisterPatientData;
    private Exception? _scenarioException;

    [When("I register a patient {string} born on {string} with phone {string} and address {string}")]
    public async Task WhenIRegisterAPatient(string fullName, string dateOfBirth, string phoneNumberFull, string addressFull)
    {
        var nameParts = fullName.Split(' ');
        var firstName = nameParts[0];
        var lastName = nameParts[1];
        var birthDate = DateOnly.Parse(dateOfBirth);
        
        // Parse phone: "+1 5551234567"
        var phoneParts = phoneNumberFull.Split(' ');
        var countryCode = phoneParts[0];
        var phoneNumber = phoneParts[1];
        
        // Parse address: "Main Street 123 A, 10001 New York"
        var addressParts = addressFull.Split(", ");
        var streetPart = addressParts[0]; // "Main Street 123 A"
        var cityPart = addressParts[1];   // "10001 New York"
        
        var streetWords = streetPart.Split(' ');
        var street = string.Join(" ", streetWords.Take(streetWords.Length - 2)); // "Main Street"
        var houseNumber = streetWords[^2]; // "123"
        var apartment = streetWords[^1];   // "A"
        
        var cityWords = cityPart.Split(' ');
        var postalCode = cityWords[0]; // "10001"
        var city = string.Join(" ", cityWords.Skip(1)); // "New York"
        
        _scenarioRegisterPatientData = new(firstName, lastName, birthDate, countryCode, phoneNumber, street, houseNumber, apartment, postalCode, city);

        try
        {
            _scenarioPatientId = await RegisterPatient(firstName, lastName, birthDate, countryCode, phoneNumber, street, houseNumber, apartment, postalCode, city);
        }
        catch (Exception ex)
        {
            _scenarioException = ex;
            _scenarioPatientId = null;
        }
    }
    
    [Then("the patient should be registered successfully")]
    public void ThenThePatientShouldBeRegisteredSuccessfully()
    {
        _scenarioPatientId.ShouldNotBeNull();
        _scenarioPatientId.ShouldNotBe(Guid.Empty);
        _scenarioException.ShouldBeNull();
    }

    [Then("I should be able to retrieve the patient details")]
    public async Task ThenIShouldBeAbleToRetrieveThePatientDetails()
    {
        _scenarioPatientId.ShouldNotBeNull();
        _scenarioRegisterPatientData.ShouldNotBeNull();

        var query = new GetPatientQuery(_scenarioPatientId.Value);
        var patient = await _dispatcher.ExecuteQuery(query);

        patient.ShouldNotBeNull();
        patient.Id.ShouldBe(_scenarioPatientId.Value);
        patient.Name.FirstName.ShouldBe(_scenarioRegisterPatientData.FirstName);
        patient.Name.LastName.ShouldBe(_scenarioRegisterPatientData.LastName);
        patient.DateOfBirth.ShouldBe(_scenarioRegisterPatientData.DateOfBirth);
        patient.PhoneNumber.CountryCode.ShouldBe(_scenarioRegisterPatientData.CountryCode);
        patient.PhoneNumber.Number.ShouldBe(_scenarioRegisterPatientData.PhoneNumber);
        patient.Address.Street.ShouldBe(_scenarioRegisterPatientData.Street);
        patient.Address.HouseNumber.ShouldBe(_scenarioRegisterPatientData.HouseNumber);
        patient.Address.Apartment.ShouldBe(_scenarioRegisterPatientData.Apartment);
        patient.Address.PostalCode.ShouldBe(_scenarioRegisterPatientData.PostalCode);
        patient.Address.City.ShouldBe(_scenarioRegisterPatientData.City);
    }
    
    private async Task<Guid> RegisterPatient(string firstName, string lastName, DateOnly dateOfBirth, string countryCode, string phoneNumber, string street, string houseNumber, string apartment, string postalCode, string city)
    {
        var command = new RegisterPatientCommand(
            new RegisterPatientCommand.PersonNameData(firstName, lastName),
            dateOfBirth,
            new RegisterPatientCommand.PhoneNumberData(countryCode, phoneNumber),
            new RegisterPatientCommand.AddressData(street, houseNumber, apartment, postalCode, city));

        return await _dispatcher.Execute(command);
    }

    private record RegisterPatientData(
        string FirstName,
        string LastName,
        DateOnly DateOfBirth,
        string CountryCode,
        string PhoneNumber,
        string Street,
        string HouseNumber,
        string Apartment,
        string PostalCode,
        string City);
}