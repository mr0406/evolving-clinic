using EvolvingClinic.Application.Patients.Commands;
using EvolvingClinic.Application.Patients.Queries;
using EvolvingClinic.BusinessTests.Utils;
using Reqnroll;
using Shouldly;

namespace EvolvingClinic.BusinessTests.StepDefinitions;

[Binding]
public sealed class RegisterPatientStepDefinitions
{
    private Guid? _scenarioPatientId;

    [When("I register a patient {string} born on {string} with phone {string} and address {string}")]
    public async Task WhenIRegisterAPatient(
        string fullName, 
        string dateOfBirth, 
        string phoneNumberFull, 
        string addressFull)
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

        _scenarioPatientId = await RegisterPatient(
            firstName,
            lastName,
            birthDate,
            countryCode,
            phoneNumber,
            street,
            houseNumber,
            apartment,
            postalCode,
            city);
    }
    
    [Given("I registered patients:")]
    public async Task GivenIRegisteredPatients(Table table)
    {
        foreach (var row in table.Rows)
        {
            var firstName = row["First Name"];
            var lastName = row["Last Name"];

            var registerCommand = new RegisterPatientCommand(
                new RegisterPatientCommand.PersonNameData(firstName, lastName),
                new DateOnly(1990, 1, 1),
                new RegisterPatientCommand.PhoneNumberData("+1", "5551234567"),
                new RegisterPatientCommand.AddressData("Test Street", "123", null, "12345", "Test City"));

            await TestDispatcher.Execute(registerCommand);
        }
    }

    [When("I register patient on {string}:")]
    public async Task WhenIRegisterPatientOn(string dateString, Table table)
    {
        var row = table.Rows[0];
        var firstName = row["First Name"];
        var lastName = row["Last Name"];
        var dateOfBirth = DateOnly.Parse(row["Date of Birth"]);
        var phoneNumberFull = row["Phone Number"];
        var streetAddress = row["Street Address"];
        var postalCode = row["Postal Code"];
        var city = row["City"];

        var phoneParts = phoneNumberFull.Split(' ');
        var countryCode = phoneParts[0];
        var phoneNumber = phoneParts[1];

        var streetParts = streetAddress.Split(' ');
        var street = string.Join(" ", streetParts.Take(streetParts.Length - 2));
        var houseNumber = streetParts[^2];
        var apartment = streetParts[^1];

        _scenarioPatientId = await RegisterPatient(
            firstName,
            lastName,
            dateOfBirth,
            countryCode,
            phoneNumber,
            street,
            houseNumber,
            apartment,
            postalCode,
            city);
    }

    [Given("patient {string} {string} is registered")]
    public async Task GivenPatientIsRegistered(string firstName, string lastName)
    {
        var registerCommand = new RegisterPatientCommand(
            new RegisterPatientCommand.PersonNameData(firstName, lastName),
            new DateOnly(1990, 1, 1),
            new RegisterPatientCommand.PhoneNumberData("+1", "5551234567"),
            new RegisterPatientCommand.AddressData("Test Street", "123", null, "12345", "Test City"));

        await TestDispatcher.Execute(registerCommand);
    }
    
    [Then("the patient should be registered successfully")]
    public void ThenThePatientShouldBeRegisteredSuccessfully()
    {
        _scenarioPatientId.ShouldNotBeNull();
        _scenarioPatientId.ShouldNotBe(Guid.Empty);
    }
    
    [Then("the registered patient should be:")]
    public async Task ThenTheRegisteredPatientShouldBe(Table table)
    {
        _scenarioPatientId.ShouldNotBeNull();

        var query = new GetPatientQuery(_scenarioPatientId.Value);
        var patient = await TestDispatcher.ExecuteQuery(query);

        patient.ShouldNotBeNull();

        var expectedRow = table.Rows[0];

        patient.Name.FirstName.ShouldBe(expectedRow["First Name"]);
        patient.Name.LastName.ShouldBe(expectedRow["Last Name"]);
        patient.DateOfBirth.ToString("yyyy-MM-dd").ShouldBe(expectedRow["Date of Birth"]);
        $"{patient.PhoneNumber.CountryCode} {patient.PhoneNumber.Number}".ShouldBe(expectedRow["Phone Number"]);
        $"{patient.Address.Street} {patient.Address.HouseNumber} {patient.Address.Apartment}".Trim().ShouldBe(expectedRow["Street Address"]);
        patient.Address.PostalCode.ShouldBe(expectedRow["Postal Code"]);
        patient.Address.City.ShouldBe(expectedRow["City"]);
    }
    
    private async Task<Guid> RegisterPatient(
        string firstName, 
        string lastName, 
        DateOnly dateOfBirth, 
        string countryCode, 
        string phoneNumber, 
        string street, 
        string houseNumber, 
        string apartment, 
        string postalCode, 
        string city)
    {
        var command = new RegisterPatientCommand(
            new RegisterPatientCommand.PersonNameData(firstName, lastName),
            dateOfBirth,
            new RegisterPatientCommand.PhoneNumberData(countryCode, phoneNumber),
            new RegisterPatientCommand.AddressData(street, houseNumber, apartment, postalCode, city));

        return await TestDispatcher.Execute(command);
    }
}