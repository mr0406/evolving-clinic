using EvolvingClinic.Domain.Patients;
using EvolvingClinic.Domain.Shared;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Patients;

[TestFixture]
public class PatientRegisterTests
{
    [Test]
    public void GivenValidPatientData_WhenRegisterPatient_ThenIsRegisteredSuccessfully()
    {
        // Given
        var name = new PersonName("John", "Smith");
        var dateOfBirth = new DateOnly(1985, 3, 20);
        var phoneNumber = new PhoneNumber("+1", "5551234567");
        var address = new Address("Main Street", "123", "A", "12345", "New York");

        // When
        var patient = Patient.Register(name, dateOfBirth, phoneNumber, address);

        // Then
        patient.ShouldNotBeNull();
        patient.Id.ShouldNotBe(Guid.Empty);

        var snapshot = patient.CreateSnapshot();
        
        snapshot.Id.ShouldBe(patient.Id);
        snapshot.Name.ShouldBe(name);
        snapshot.DateOfBirth.ShouldBe(dateOfBirth);
        snapshot.PhoneNumber.ShouldBe(phoneNumber);
        snapshot.Address.ShouldBe(address);
    }

    [Test]
    public void GivenPatientWithoutApartment_WhenRegisterPatient_ThenIsRegisteredSuccessfully()
    {
        // Given
        var name = new PersonName("Alice", "Johnson");
        var dateOfBirth = new DateOnly(1990, 7, 15);
        var phoneNumber = new PhoneNumber("+48", "123456789");
        var address = new Address("Oak Street", "456", null, "54321", "Warsaw");

        // When
        var patient = Patient.Register(name, dateOfBirth, phoneNumber, address);

        // Then
        patient.ShouldNotBeNull();
        patient.Id.ShouldNotBe(Guid.Empty);

        var snapshot = patient.CreateSnapshot();
        snapshot.Address.Apartment.ShouldBeNull();
    }
}