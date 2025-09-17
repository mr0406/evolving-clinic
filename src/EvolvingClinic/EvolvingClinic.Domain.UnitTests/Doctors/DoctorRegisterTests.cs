using EvolvingClinic.Domain.Doctors;
using EvolvingClinic.Domain.Shared;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Doctors;

public class DoctorRegisterTests : TestBase
{
    [Test]
    public void GivenValidDoctorData_WhenRegisterDoctor_ThenIsRegisteredSuccessfully()
    {
        // Given
        var code = "SMITH";
        var name = new PersonName("John", "Smith");
        var existingCodes = new List<string> { "JONES", "WILLIAMS" };

        // When
        var doctor = Doctor.Register(code, name, existingCodes);

        // Then
        doctor.ShouldNotBeNull();
        doctor.Code.ShouldBe(code);

        var snapshot = doctor.CreateSnapshot();
        snapshot.Code.ShouldBe(code);
        snapshot.Name.ShouldBe(name);
    }

    [Test]
    public void GivenLowercaseCode_WhenRegisterDoctor_ThenCodeIsUppercase()
    {
        // Given
        var code = "smith";
        var name = new PersonName("John", "Smith");
        var existingCodes = new List<string>();

        // When
        var doctor = Doctor.Register(code, name, existingCodes);

        // Then
        var snapshot = doctor.CreateSnapshot();
        snapshot.Code.ShouldBe("SMITH");
    }

    [Test]
    public void GivenCodeWithWhitespace_WhenRegisterDoctor_ThenTrimsWhitespace()
    {
        // Given
        var code = "  SMITH  ";
        var name = new PersonName("John", "Smith");
        var existingCodes = new List<string>();

        // When
        var doctor = Doctor.Register(code, name, existingCodes);

        // Then
        var snapshot = doctor.CreateSnapshot();
        snapshot.Code.ShouldBe("SMITH");
    }

    [Test]
    public void GivenDuplicateCode_WhenRegisterDoctor_ThenThrowsArgumentException()
    {
        // Given
        var code = "SMITH";
        var name = new PersonName("Jane", "Smith");
        var existingCodes = new List<string> { "SMITH", "JONES" };

        // When & Then
        var exception = Assert.Throws<ArgumentException>(() =>
            Doctor.Register(code, name, existingCodes));

        exception!.Message.ShouldBe("A doctor with the code 'SMITH' already exists");
    }

    [Test]
    public void GivenDuplicateCodeDifferentCase_WhenRegisterDoctor_ThenThrowsArgumentException()
    {
        // Given
        var code = "smith";
        var name = new PersonName("Jane", "Smith");
        var existingCodes = new List<string> { "SMITH", "JONES" };

        // When & Then
        var exception = Assert.Throws<ArgumentException>(() =>
            Doctor.Register(code, name, existingCodes));

        exception!.Message.ShouldBe("A doctor with the code 'SMITH' already exists");
    }

    [Test]
    public void GivenEmptyCode_WhenRegisterDoctor_ThenThrowsArgumentException()
    {
        // Given
        var code = "";
        var name = new PersonName("John", "Smith");
        var existingCodes = new List<string>();

        // When & Then
        var exception = Assert.Throws<ArgumentException>(() =>
            Doctor.Register(code, name, existingCodes));

        exception!.Message.ShouldBe("Doctor code is required");
    }

    [Test]
    public void GivenWhitespaceCode_WhenRegisterDoctor_ThenThrowsArgumentException()
    {
        // Given
        var code = "   ";
        var name = new PersonName("John", "Smith");
        var existingCodes = new List<string>();

        // When & Then
        var exception = Assert.Throws<ArgumentException>(() =>
            Doctor.Register(code, name, existingCodes));

        exception!.Message.ShouldBe("Doctor code is required");
    }

    [Test]
    public void GivenNullCode_WhenRegisterDoctor_ThenThrowsArgumentException()
    {
        // Given
        string code = null!;
        var name = new PersonName("John", "Smith");
        var existingCodes = new List<string>();

        // When & Then
        var exception = Assert.Throws<ArgumentException>(() =>
            Doctor.Register(code, name, existingCodes));

        exception!.Message.ShouldBe("Doctor code is required");
    }

    [Test]
    public void GivenValidPersonName_WhenRegisterDoctor_ThenNameIsPreserved()
    {
        // Given
        var code = "SMITH";
        var name = new PersonName("John", "Smith");
        var existingCodes = new List<string>();

        // When
        var doctor = Doctor.Register(code, name, existingCodes);

        // Then
        var snapshot = doctor.CreateSnapshot();
        snapshot.Name.FirstName.ShouldBe("John");
        snapshot.Name.LastName.ShouldBe("Smith");
        snapshot.Name.FullName.ShouldBe("John Smith");
    }

    [Test]
    public void GivenEmptyExistingCodesList_WhenRegisterDoctor_ThenIsCreatedSuccessfully()
    {
        // Given
        var code = "SMITH";
        var name = new PersonName("John", "Smith");
        var existingCodes = new List<string>();

        // When
        var doctor = Doctor.Register(code, name, existingCodes);

        // Then
        doctor.ShouldNotBeNull();
        doctor.Code.ShouldBe(code);
    }

    [Test]
    public void GivenMultipleExistingCodes_WhenRegisterUniqueDoctor_ThenIsCreatedSuccessfully()
    {
        // Given
        var code = "BROWN";
        var name = new PersonName("Michael", "Brown");
        var existingCodes = new List<string> { "SMITH", "JONES", "WILLIAMS", "DAVIS" };

        // When
        var doctor = Doctor.Register(code, name, existingCodes);

        // Then
        doctor.ShouldNotBeNull();
        doctor.Code.ShouldBe(code);
    }
}