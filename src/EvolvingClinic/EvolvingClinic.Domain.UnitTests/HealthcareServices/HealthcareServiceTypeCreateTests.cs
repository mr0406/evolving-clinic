using EvolvingClinic.Domain.HealthcareServices;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.HealthcareServices;

[TestFixture]
public class HealthcareServiceTypeCreateTests
{
    [Test]
    public void GivenValidServiceData_WhenCreateHealthcareServiceType_ThenIsCreatedSuccessfully()
    {
        // Given
        var name = "Routine Check-up";
        var code = "RCU";
        var duration = TimeSpan.FromMinutes(30);
        var existingNames = new List<string> { "Blood Test", "X-Ray" };
        var existingCodes = new List<string> { "BT", "XR" };

        // When
        var serviceType = HealthcareServiceType.Create(name, code, duration, existingNames, existingCodes);

        // Then
        serviceType.ShouldNotBeNull();
        serviceType.Id.ShouldNotBe(Guid.Empty);

        var snapshot = serviceType.CreateSnapshot();
        snapshot.Name.ShouldBe(name);
        snapshot.Code.ShouldBe(code.ToUpperInvariant());
        snapshot.Duration.ShouldBe(duration);
    }

    [Test]
    public void GivenLowercaseCode_WhenCreateHealthcareServiceType_ThenCodeIsUppercase()
    {
        // Given
        var name = "Blood Test";
        var code = "blt";
        var duration = TimeSpan.FromMinutes(15);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        var serviceType = HealthcareServiceType.Create(name, code, duration, existingNames, existingCodes);

        // Then
        var snapshot = serviceType.CreateSnapshot();
        snapshot.Code.ShouldBe("BLT");
    }

    [Test]
    public void GivenNameWithWhitespace_WhenCreateHealthcareServiceType_ThenTrimsWhitespace()
    {
        // Given
        var name = "  Routine Check-up  ";
        var code = "  RCU  ";
        var duration = TimeSpan.FromMinutes(30);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        var serviceType = HealthcareServiceType.Create(name, code, duration, existingNames, existingCodes);

        // Then
        var snapshot = serviceType.CreateSnapshot();
        snapshot.Name.ShouldBe("Routine Check-up");
        snapshot.Code.ShouldBe("RCU");
    }

    [Test]
    public void GivenMinimumDuration_WhenCreateHealthcareServiceType_ThenIsCreatedSuccessfully()
    {
        // Given
        var name = "Quick Consultation";
        var code = "QC";
        var duration = TimeSpan.FromMinutes(15);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        var serviceType = HealthcareServiceType.Create(name, code, duration, existingNames, existingCodes);

        // Then
        serviceType.ShouldNotBeNull();
        serviceType.CreateSnapshot().Duration.ShouldBe(duration);
    }

    [Test]
    public void GivenMaximumDuration_WhenCreateHealthcareServiceType_ThenIsCreatedSuccessfully()
    {
        // Given
        var name = "Complex Surgery";
        var code = "CS";
        var duration = TimeSpan.FromHours(8);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        var serviceType = HealthcareServiceType.Create(name, code, duration, existingNames, existingCodes);

        // Then
        serviceType.ShouldNotBeNull();
        serviceType.CreateSnapshot().Duration.ShouldBe(duration);
    }

    [Test]
    public void GivenDuplicateName_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "Blood Test";
        var code = "BT2";
        var duration = TimeSpan.FromMinutes(30);
        var existingNames = new List<string> { "Blood Test", "X-Ray" };
        var existingCodes = new List<string> { "BT", "XR" };

        // When & Then
        var exception = Assert.Throws<ArgumentException>(() =>
            HealthcareServiceType.Create(name, code, duration, existingNames, existingCodes));

        exception!.Message.ShouldBe("A service with the name 'Blood Test' already exists");
    }

    [Test]
    public void GivenDuplicateNameDifferentCase_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "BLOOD TEST";
        var code = "BT2";
        var duration = TimeSpan.FromMinutes(30);
        var existingNames = new List<string> { "Blood Test", "X-Ray" };
        var existingCodes = new List<string> { "BT", "XR" };

        // When & Then
        var exception = Assert.Throws<ArgumentException>(() =>
            HealthcareServiceType.Create(name, code, duration, existingNames, existingCodes));

        exception!.Message.ShouldBe("A service with the name 'BLOOD TEST' already exists");
    }

    [Test]
    public void GivenDuplicateCode_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "New Blood Test";
        var code = "BT";
        var duration = TimeSpan.FromMinutes(30);
        var existingNames = new List<string> { "Blood Test", "X-Ray" };
        var existingCodes = new List<string> { "BT", "XR" };

        // When & Then
        var exception = Assert.Throws<ArgumentException>(() =>
            HealthcareServiceType.Create(name, code, duration, existingNames, existingCodes));

        exception!.Message.ShouldBe("A service with the code 'BT' already exists");
    }

    [Test]
    public void GivenDuplicateCodeDifferentCase_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "New Blood Test";
        var code = "bt";
        var duration = TimeSpan.FromMinutes(30);
        var existingNames = new List<string> { "Blood Test", "X-Ray" };
        var existingCodes = new List<string> { "BT", "XR" };

        // When & Then
        var exception = Assert.Throws<ArgumentException>(() =>
            HealthcareServiceType.Create(name, code, duration, existingNames, existingCodes));

        exception!.Message.ShouldBe("A service with the code 'BT' already exists");
    }

    [Test]
    public void GivenEmptyName_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "";
        var code = "TC";
        var duration = TimeSpan.FromMinutes(30);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When & Then
        var exception = Assert.Throws<ArgumentException>(() =>
            HealthcareServiceType.Create(name, code, duration, existingNames, existingCodes));

        exception!.Message.ShouldBe("Service name is required");
    }

    [Test]
    public void GivenWhitespaceName_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "   ";
        var code = "TC";
        var duration = TimeSpan.FromMinutes(30);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When & Then
        var exception = Assert.Throws<ArgumentException>(() =>
            HealthcareServiceType.Create(name, code, duration, existingNames, existingCodes));

        exception!.Message.ShouldBe("Service name is required");
    }

    [Test]
    public void GivenEmptyCode_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "Test Service";
        var code = "";
        var duration = TimeSpan.FromMinutes(30);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When & Then
        var exception = Assert.Throws<ArgumentException>(() =>
            HealthcareServiceType.Create(name, code, duration, existingNames, existingCodes));

        exception!.Message.ShouldBe("Service code is required");
    }

    [Test]
    public void GivenWhitespaceCode_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "Test Service";
        var code = "   ";
        var duration = TimeSpan.FromMinutes(30);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When & Then
        var exception = Assert.Throws<ArgumentException>(() =>
            HealthcareServiceType.Create(name, code, duration, existingNames, existingCodes));

        exception!.Message.ShouldBe("Service code is required");
    }

    [Test]
    public void GivenDurationTooShort_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "Quick Test";
        var code = "QT";
        var duration = TimeSpan.FromMinutes(14);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When & Then
        var exception = Assert.Throws<ArgumentException>(() =>
            HealthcareServiceType.Create(name, code, duration, existingNames, existingCodes));

        exception!.Message.ShouldBe("Service duration must be at least 15 minutes");
    }

    [Test]
    public void GivenDurationTooLong_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "Marathon Surgery";
        var code = "MS";
        var duration = TimeSpan.FromHours(9);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When & Then
        var exception = Assert.Throws<ArgumentException>(() =>
            HealthcareServiceType.Create(name, code, duration, existingNames, existingCodes));

        exception!.Message.ShouldBe("Service duration cannot exceed 8 hours");
    }
}