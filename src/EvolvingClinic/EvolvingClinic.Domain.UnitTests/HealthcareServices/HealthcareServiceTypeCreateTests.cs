using EvolvingClinic.Domain.HealthcareServices;
using EvolvingClinic.Domain.Shared;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.HealthcareServices;

public class HealthcareServiceTypeCreateTests : TestBase
{
    [Test]
    public void GivenValidServiceData_WhenCreateHealthcareServiceType_ThenIsCreatedSuccessfully()
    {
        // Given
        var name = "Routine Check-up";
        var code = "RCU";
        var duration = TimeSpan.FromMinutes(30);
        var price = new Money(100.00m);
        var existingNames = new List<string> { "Blood Test", "X-Ray" };
        var existingCodes = new List<string> { "BT", "XR" };

        // When
        var serviceType = HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

        // Then
        var expectedHistory = new[]
        {
            new HealthcareServiceType.PriceHistoryEntry(price, DateOnly.FromDateTime(DateTime.Now.Date), null)
        };
        
        serviceType.ShouldNotBeNull();
        serviceType.Code.ShouldBe(code);

        var snapshot = serviceType.CreateSnapshot();
        snapshot.Code.ShouldBe(code);
        snapshot.Name.ShouldBe(name);
        snapshot.Duration.ShouldBe(duration);
        snapshot.Price.ShouldBe(price);
        snapshot.PriceHistory.ShouldBe(expectedHistory);
    }

    [Test]
    public void GivenLowercaseCode_WhenCreateHealthcareServiceType_ThenCodeIsUppercase()
    {
        // Given
        var name = "Blood Test";
        var code = "blt";
        var duration = TimeSpan.FromMinutes(15);
        var price = new Money(50.00m);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        var serviceType = HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

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
        var price = new Money(75.00m);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        var serviceType = HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

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
        var price = new Money(25.00m);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        var serviceType = HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

        // Then
        serviceType.ShouldNotBeNull();
        serviceType.Code.ShouldBe(code);
        serviceType.CreateSnapshot().Duration.ShouldBe(duration);
    }

    [Test]
    public void GivenMaximumDuration_WhenCreateHealthcareServiceType_ThenIsCreatedSuccessfully()
    {
        // Given
        var name = "Complex Surgery";
        var code = "CS";
        var duration = TimeSpan.FromHours(8);
        var price = new Money(5000.00m);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        var serviceType = HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

        // Then
        serviceType.ShouldNotBeNull();
        serviceType.Code.ShouldBe(code);
        serviceType.CreateSnapshot().Duration.ShouldBe(duration);
    }

    [Test]
    public void GivenDuplicateName_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "Blood Test";
        var code = "BT2";
        var duration = TimeSpan.FromMinutes(30);
        var price = new Money(40.00m);
        var existingNames = new List<string> { "Blood Test", "X-Ray" };
        var existingCodes = new List<string> { "BT", "XR" };

        // When
        Action createService = () => HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

        // Then
        var exception = Should.Throw<ArgumentException>(createService);
        exception!.Message.ShouldBe("A service with the name 'Blood Test' already exists");
    }

    [Test]
    public void GivenDuplicateNameDifferentCase_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "BLOOD TEST";
        var code = "BT2";
        var duration = TimeSpan.FromMinutes(30);
        var price = new Money(40.00m);
        var existingNames = new List<string> { "Blood Test", "X-Ray" };
        var existingCodes = new List<string> { "BT", "XR" };

        // When
        Action createService = () => HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

        // Then
        var exception = Should.Throw<ArgumentException>(createService);
        exception!.Message.ShouldBe("A service with the name 'BLOOD TEST' already exists");
    }

    [Test]
    public void GivenDuplicateCode_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "New Blood Test";
        var code = "BT";
        var duration = TimeSpan.FromMinutes(30);
        var price = new Money(45.00m);
        var existingNames = new List<string> { "Blood Test", "X-Ray" };
        var existingCodes = new List<string> { "BT", "XR" };

        // When
        Action createService = () => HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

        // Then
        var exception = Should.Throw<ArgumentException>(createService);
        exception!.Message.ShouldBe("A service with the code 'BT' already exists");
    }

    [Test]
    public void GivenDuplicateCodeDifferentCase_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "New Blood Test";
        var code = "bt";
        var duration = TimeSpan.FromMinutes(30);
        var price = new Money(45.00m);
        var existingNames = new List<string> { "Blood Test", "X-Ray" };
        var existingCodes = new List<string> { "BT", "XR" };

        // When
        Action createService = () => HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

        // Then
        var exception = Should.Throw<ArgumentException>(createService);
        exception!.Message.ShouldBe("A service with the code 'BT' already exists");
    }

    [Test]
    public void GivenEmptyName_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "";
        var code = "TC";
        var duration = TimeSpan.FromMinutes(30);
        var price = new Money(60.00m);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        Action createService = () => HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

        // Then
        var exception = Should.Throw<ArgumentException>(createService);
        exception!.Message.ShouldBe("Service name is required");
    }

    [Test]
    public void GivenWhitespaceName_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "   ";
        var code = "TC";
        var duration = TimeSpan.FromMinutes(30);
        var price = new Money(60.00m);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        Action createService = () => HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

        // Then
        var exception = Should.Throw<ArgumentException>(createService);
        exception!.Message.ShouldBe("Service name is required");
    }

    [Test]
    public void GivenEmptyCode_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "Test Service";
        var code = "";
        var duration = TimeSpan.FromMinutes(30);
        var price = new Money(80.00m);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        Action createService = () => HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

        // Then
        var exception = Should.Throw<ArgumentException>(createService);
        exception!.Message.ShouldBe("Service code is required");
    }

    [Test]
    public void GivenWhitespaceCode_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "Test Service";
        var code = "   ";
        var duration = TimeSpan.FromMinutes(30);
        var price = new Money(80.00m);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        Action createService = () => HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

        // Then
        var exception = Should.Throw<ArgumentException>(createService);
        exception!.Message.ShouldBe("Service code is required");
    }

    [Test]
    public void GivenDurationTooShort_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "Quick Test";
        var code = "QT";
        var duration = TimeSpan.FromMinutes(14);
        var price = new Money(20.00m);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        Action createService = () => HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

        // Then
        var exception = Should.Throw<ArgumentException>(createService);
        exception!.Message.ShouldBe("Service duration must be at least 15 minutes");
    }

    [Test]
    public void GivenDurationTooLong_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "Marathon Surgery";
        var code = "MS";
        var duration = TimeSpan.FromHours(9);
        var price = new Money(10000.00m);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        Action createService = () => HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

        // Then
        var exception = Should.Throw<ArgumentException>(createService);
        exception!.Message.ShouldBe("Service duration cannot exceed 8 hours");
    }

    [Test]
    public void GivenZeroPrice_WhenCreateHealthcareServiceType_ThenIsCreatedSuccessfully()
    {
        // Given
        var name = "Free Consultation";
        var code = "FC";
        var duration = TimeSpan.FromMinutes(30);
        var price = new Money(0.00m);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        var serviceType = HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

        // Then
        serviceType.ShouldNotBeNull();
        serviceType.CreateSnapshot().Price.ShouldBe(price);
    }

    [Test]
    public void GivenNegativePrice_WhenCreateHealthcareServiceType_ThenThrowsArgumentException()
    {
        // Given
        var name = "Test Service";
        var code = "TS";
        var duration = TimeSpan.FromMinutes(30);
        var price = new Money(-10.00m);
        var existingNames = new List<string>();
        var existingCodes = new List<string>();

        // When
        Action createService = () => HealthcareServiceType.Create(name, code, duration, price, existingNames, existingCodes);

        // Then
        var exception = Should.Throw<ArgumentException>(createService);
        exception!.Message.ShouldBe("Service price must be 0 or greater");
    }
}