using EvolvingClinic.Domain.Shared;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Shared;

public class PersonNameCreateTests : TestBase
{
    [Test]
    public void GivenValidFirstAndLastName_WhenCreatePersonName_ThenIsCreatedSuccessfully()
    {
        // Given
        var firstName = "John";
        var lastName = "Smith";

        // When
        var personName = new PersonName(firstName, lastName);

        // Then
        personName.FirstName.ShouldBe(firstName);
        personName.LastName.ShouldBe(lastName);
    }

    [Test]
    public void GivenEmptyFirstName_WhenCreatePersonName_ThenThrowsArgumentException()
    {
        // Given
        var firstName = "";
        var lastName = "Smith";

        // When
        Action createPersonName = () =>
            new PersonName(firstName, lastName);

        // Then
        var exception = Should.Throw<ArgumentException>(createPersonName);
        exception!.Message.ShouldBe("First name is required");
    }

    [Test]
    public void GivenNullFirstName_WhenCreatePersonName_ThenThrowsArgumentException()
    {
        // Given
        string firstName = null!;
        var lastName = "Smith";

        // When
        Action createPersonName = () =>
            new PersonName(firstName, lastName);

        // Then
        var exception = Should.Throw<ArgumentException>(createPersonName);
        exception!.Message.ShouldBe("First name is required");
    }

    [Test]
    public void GivenEmptyLastName_WhenCreatePersonName_ThenThrowsArgumentException()
    {
        // Given
        var firstName = "John";
        var lastName = "";

        // When
        Action createPersonName = () =>
            new PersonName(firstName, lastName);

        // Then
        var exception = Should.Throw<ArgumentException>(createPersonName);
        exception!.Message.ShouldBe("Last name is required");
    }

    [Test]
    public void GivenNullLastName_WhenCreatePersonName_ThenThrowsArgumentException()
    {
        // Given
        var firstName = "John";
        string lastName = null!;

        // When
        Action createPersonName = () =>
            new PersonName(firstName, lastName);

        // Then
        var exception = Should.Throw<ArgumentException>(createPersonName);
        exception!.Message.ShouldBe("Last name is required");
    }
}