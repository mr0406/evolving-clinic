using EvolvingClinic.Domain.Shared;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Shared;

public class MoneyCreateTests : TestBase
{
    [Test]
    public void GivenPositiveDecimal_WhenCreateMoney_ThenValueIsSet()
    {
        // Given
        var value = 123.45m;

        // When
        var money = new Money(value);

        // Then
        money.Value.ShouldBe(123.45m);
    }

    [Test]
    public void GivenZeroDecimal_WhenCreateMoney_ThenValueIsSet()
    {
        // Given
        var value = 0.00m;

        // When
        var money = new Money(value);

        // Then
        money.Value.ShouldBe(0.00m);
    }

    [Test]
    public void GivenNegativeDecimal_WhenCreateMoney_ThenValueIsSet()
    {
        // Given
        var value = -50.00m;

        // When
        var money = new Money(value);

        // Then
        money.Value.ShouldBe(-50.00m);
    }

    [Test]
    public void GivenDecimalWithMoreThanTwoDecimalPlaces_WhenCreateMoney_ThenIsRoundedToTwoDecimalPlaces()
    {
        // Given
        var value = 123.456m;

        // When
        var money = new Money(value);

        // Then
        money.Value.ShouldBe(123.46m);
    }

    [Test]
    public void GivenDecimalWithMidpointValue_WhenCreateMoney_ThenRoundsAwayFromZero()
    {
        // Given
        var value = 123.455m;

        // When
        var money = new Money(value);

        // Then
        money.Value.ShouldBe(123.46m);
    }

    [Test]
    public void GivenNegativeDecimalWithMidpointValue_WhenCreateMoney_ThenRoundsAwayFromZero()
    {
        // Given
        var value = -123.455m;

        // When
        var money = new Money(value);

        // Then
        money.Value.ShouldBe(-123.46m);
    }

    [Test]
    public void GivenMoney_WhenCallToString_ThenReturnsFormattedCurrency()
    {
        // Given
        var money = new Money(123.45m);

        // When
        var result = money.ToString();

        // Then
        result.ShouldBe("$123.45");
    }

    [Test]
    public void GivenZeroMoney_WhenCallToString_ThenReturnsFormattedCurrency()
    {
        // Given
        var money = new Money(0.00m);

        // When
        var result = money.ToString();

        // Then
        result.ShouldBe("$0.00");
    }

    [Test]
    public void GivenNegativeMoney_WhenCallToString_ThenReturnsFormattedCurrency()
    {
        // Given
        var money = new Money(-50.00m);

        // When
        var result = money.ToString();

        // Then
        result.ShouldBe("$-50.00");
    }

    [Test]
    public void GivenTwoMoneyWithSameValue_WhenCompareEquality_ThenAreEqual()
    {
        // Given
        var money1 = new Money(100.00m);
        var money2 = new Money(100.00m);

        // Then
        money1.ShouldBe(money2);
        (money1 == money2).ShouldBeTrue();
        money1.Equals(money2).ShouldBeTrue();
    }

    [Test]
    public void GivenTwoMoneyWithDifferentValues_WhenCompareEquality_ThenAreNotEqual()
    {
        // Given
        var money1 = new Money(100.00m);
        var money2 = new Money(200.00m);

        // Then
        money1.ShouldNotBe(money2);
        (money1 == money2).ShouldBeFalse();
        money1.Equals(money2).ShouldBeFalse();
    }

    [Test]
    public void GivenTwoMoneyWithSameValueButDifferentPrecision_WhenCompareEquality_ThenAreEqual()
    {
        // Given
        var money1 = new Money(100.00m);
        var money2 = new Money(100.0m);

        // Then
        money1.ShouldBe(money2);
    }
}