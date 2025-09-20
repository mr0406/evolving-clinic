using EvolvingClinic.Domain.Shared;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Shared;

public class AddressCreateTests : TestBase
{
    [Test]
    public void GivenValidAddressComponents_WhenCreateAddress_ThenIsCreatedSuccessfully()
    {
        // Given
        var street = "Main Street";
        var houseNumber = "123";
        var apartment = "A";
        var postalCode = "12345";
        var city = "Warsaw";

        // When
        var address = new Address(street, houseNumber, apartment, postalCode, city);

        // Then
        address.Street.ShouldBe(street);
        address.HouseNumber.ShouldBe(houseNumber);
        address.Apartment.ShouldBe(apartment);
        address.PostalCode.ShouldBe(postalCode);
        address.City.ShouldBe(city);
    }

    [Test]
    public void GivenNoApartment_WhenCreateAddress_ThenIsCreatedSuccessfully()
    {
        // Given
        var street = "Main Street";
        var houseNumber = "123";
        string? apartment = null;
        var postalCode = "12345";
        var city = "Warsaw";

        // When
        var address = new Address(street, houseNumber, apartment, postalCode, city);

        // Then
        address.Street.ShouldBe(street);
        address.HouseNumber.ShouldBe(houseNumber);
        address.Apartment.ShouldBe(apartment);
        address.PostalCode.ShouldBe(postalCode);
        address.City.ShouldBe(city);
    }

    [Test]
    public void GivenApartmentWithNumbers_WhenCreateAddress_ThenIsCreatedSuccessfully()
    {
        // Given
        var street = "Main Street";
        var houseNumber = "123";
        var apartment = "15D";
        var postalCode = "12345";
        var city = "Warsaw";

        // When
        var address = new Address(street, houseNumber, apartment, postalCode, city);

        // Then
        address.Apartment.ShouldBe(apartment);
    }

    [Test]
    public void GivenEmptyStreet_WhenCreateAddress_ThenThrowsArgumentException()
    {
        // Given
        var street = "";
        var houseNumber = "123";
        var apartment = "A";
        var postalCode = "12345";
        var city = "Warsaw";

        // When
        var exception = Should.Throw<ArgumentException>(() => 
            new Address(street, houseNumber, apartment, postalCode, city));
        
        // Then
        exception!.Message.ShouldBe("Street is required");
    }

    [Test]
    public void GivenNullStreet_WhenCreateAddress_ThenThrowsArgumentException()
    {
        // Given
        string street = null!;
        var houseNumber = "123";
        var apartment = "A";
        var postalCode = "12345";
        var city = "Warsaw";

        // When
        var exception = Should.Throw<ArgumentException>(() => 
            new Address(street, houseNumber, apartment, postalCode, city));
        
        // Then
        exception!.Message.ShouldBe("Street is required");
    }

    [Test]
    public void GivenEmptyHouseNumber_WhenCreateAddress_ThenThrowsArgumentException()
    {
        // Given
        var street = "Main Street";
        var houseNumber = "";
        var apartment = "A";
        var postalCode = "12345";
        var city = "Warsaw";

        // When
        var exception = Should.Throw<ArgumentException>(() => 
            new Address(street, houseNumber, apartment, postalCode, city));
        
        // Then
        exception!.Message.ShouldBe("House number is required");
    }

    [Test]
    public void GivenNullHouseNumber_WhenCreateAddress_ThenThrowsArgumentException()
    {
        // Given
        var street = "Main Street";
        string houseNumber = null!;
        var apartment = "A";
        var postalCode = "12345";
        var city = "Warsaw";

        // When
        var exception = Should.Throw<ArgumentException>(() => 
            new Address(street, houseNumber, apartment, postalCode, city));
        
        // Then
        exception!.Message.ShouldBe("House number is required");
    }

    [Test]
    public void GivenEmptyPostalCode_WhenCreateAddress_ThenThrowsArgumentException()
    {
        // Given
        var street = "Main Street";
        var houseNumber = "123";
        var apartment = "A";
        var postalCode = "";
        var city = "Warsaw";

        // When
        var exception = Should.Throw<ArgumentException>(() => 
            new Address(street, houseNumber, apartment, postalCode, city));
        
        // Then
        exception!.Message.ShouldBe("Postal code is required");
    }

    [Test]
    public void GivenNullPostalCode_WhenCreateAddress_ThenThrowsArgumentException()
    {
        // Given
        var street = "Main Street";
        var houseNumber = "123";
        var apartment = "A";
        string postalCode = null!;
        var city = "Warsaw";

        // When
        var exception = Should.Throw<ArgumentException>(() => 
            new Address(street, houseNumber, apartment, postalCode, city));
        
        // Then
        exception!.Message.ShouldBe("Postal code is required");
    }

    [Test]
    public void GivenEmptyCity_WhenCreateAddress_ThenThrowsArgumentException()
    {
        // Given
        var street = "Main Street";
        var houseNumber = "123";
        var apartment = "A";
        var postalCode = "12345";
        var city = "";

        // When
        var exception = Should.Throw<ArgumentException>(() => 
            new Address(street, houseNumber, apartment, postalCode, city));
        
        // Then
        exception!.Message.ShouldBe("City is required");
    }

    [Test]
    public void GivenNullCity_WhenCreateAddress_ThenThrowsArgumentException()
    {
        // Given
        var street = "Main Street";
        var houseNumber = "123";
        var apartment = "A";
        var postalCode = "12345";
        string city = null!;

        // When
        var exception = Should.Throw<ArgumentException>(() => 
            new Address(street, houseNumber, apartment, postalCode, city));
        
        // Then
        exception!.Message.ShouldBe("City is required");
    }
}