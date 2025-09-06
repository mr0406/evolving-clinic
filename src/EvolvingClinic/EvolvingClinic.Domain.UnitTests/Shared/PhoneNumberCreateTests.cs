using EvolvingClinic.Domain.Shared;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Shared;

[TestFixture]
public class PhoneNumberCreateTests
{
    [Test]
    public void GivenValidCountryCodeAndNumber_WhenCreatePhoneNumber_ThenIsCreatedSuccessfully()
    {
        // Given
        var countryCode = "+1";
        var number = "5551234567";

        // When
        var phoneNumber = new PhoneNumber(countryCode, number);

        // Then
        phoneNumber.CountryCode.ShouldBe(countryCode);
        phoneNumber.Number.ShouldBe(number);
    }

    [Test]
    public void GivenPolishPhoneNumber_WhenCreatePhoneNumber_ThenIsCreatedSuccessfully()
    {
        // Given
        var countryCode = "+48";
        var number = "123456789";

        // When
        var phoneNumber = new PhoneNumber(countryCode, number);

        // Then
        phoneNumber.CountryCode.ShouldBe(countryCode);
        phoneNumber.Number.ShouldBe(number);
    }

    [Test]
    public void GivenEmptyCountryCode_WhenCreatePhoneNumber_ThenThrowsArgumentException()
    {
        // Given
        var countryCode = "";
        var number = "5551234567";

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            new PhoneNumber(countryCode, number));
        
        // Then
        exception!.Message.ShouldBe("Country code is required");
    }

    [Test]
    public void GivenNullCountryCode_WhenCreatePhoneNumber_ThenThrowsArgumentException()
    {
        // Given
        string countryCode = null!;
        var number = "5551234567";

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            new PhoneNumber(countryCode, number));
        
        // Then
        exception!.Message.ShouldBe("Country code is required");
    }

    [Test]
    public void GivenCountryCodeWithoutPlus_WhenCreatePhoneNumber_ThenThrowsArgumentException()
    {
        // Given
        var countryCode = "1";
        var number = "5551234567";

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            new PhoneNumber(countryCode, number));
        
        // Then
        exception!.Message.ShouldBe("Country code must start with + followed by digits");
    }

    [Test]
    public void GivenEmptyNumber_WhenCreatePhoneNumber_ThenThrowsArgumentException()
    {
        // Given
        var countryCode = "+1";
        var number = "";

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            new PhoneNumber(countryCode, number));
        
        // Then
        exception!.Message.ShouldBe("Phone number is required");
    }

    [Test]
    public void GivenNullNumber_WhenCreatePhoneNumber_ThenThrowsArgumentException()
    {
        // Given
        var countryCode = "+1";
        string number = null!;

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            new PhoneNumber(countryCode, number));
        
        // Then
        exception!.Message.ShouldBe("Phone number is required");
    }

    [Test]
    public void GivenNumberWithNonDigits_WhenCreatePhoneNumber_ThenThrowsArgumentException()
    {
        // Given
        var countryCode = "+1";
        var number = "555-123-4567";

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            new PhoneNumber(countryCode, number));
        
        // Then
        exception!.Message.ShouldBe("Phone number must contain only digits");
    }

    [Test]
    public void GivenNumberWithLetters_WhenCreatePhoneNumber_ThenThrowsArgumentException()
    {
        // Given
        var countryCode = "+1";
        var number = "555HELP";

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            new PhoneNumber(countryCode, number));
        
        // Then
        exception!.Message.ShouldBe("Phone number must contain only digits");
    }
}