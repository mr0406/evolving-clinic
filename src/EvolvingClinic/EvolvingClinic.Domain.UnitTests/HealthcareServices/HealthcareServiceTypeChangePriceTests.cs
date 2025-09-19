using EvolvingClinic.Domain.HealthcareServices;
using EvolvingClinic.Domain.Shared;
using EvolvingClinic.Domain.Utils;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.HealthcareServices;

public class HealthcareServiceTypeChangePriceTests : TestBase
{
    [Test]
    public void GivenMultiplePriceChangesAcrossDays_WhenChangePrice_ThenCreatesHistoryWithCorrectEndDates()
    {
        // Given
        var twoDaysAgo = new DateOnly(2024, 9, 14);
        var twoDaysAgoPrice = new Money(80.00m);
        
        var yesterday = new DateOnly(2024, 9, 15);
        var yesterdayPrice = new Money(100.00m);
        
        var today = new DateOnly(2024, 9, 16);
        var todayPrice = new Money(110.00m);
        
        ApplicationClock.SetDate(twoDaysAgo);
        var serviceType = CreateDefaultService(twoDaysAgoPrice);
        
        ApplicationClock.SetDate(yesterday);
        serviceType.ChangePrice(yesterdayPrice);

        // When
        ApplicationClock.SetDate(today);
        serviceType.ChangePrice(todayPrice);

        // Then
        var snapshot = serviceType.CreateSnapshot();

        var expectedHistory = new[]
        {
            new HealthcareServiceType.PriceHistoryEntry(twoDaysAgoPrice, twoDaysAgo, twoDaysAgo),
            new HealthcareServiceType.PriceHistoryEntry(yesterdayPrice, yesterday, yesterday),
            new HealthcareServiceType.PriceHistoryEntry(todayPrice, today, null)
        };

        snapshot.Price.ShouldBe(todayPrice);
        snapshot.PriceHistory.ShouldBe(expectedHistory);
    }
    
    [Test]
    public void GivenSameDayPriceChange_WhenChangePrice_ThenOverridesPreviousChange()
    {
        // Given
        var testDate = new DateOnly(2024, 9, 16);
        ApplicationClock.SetDate(testDate);

        var initialPrice = new Money(100.00m);
        var serviceType = CreateDefaultService(initialPrice);
        var firstPrice = new Money(120.00m);
        var correctedPrice = new Money(130.00m);

        // When
        serviceType.ChangePrice(firstPrice);
        serviceType.ChangePrice(correctedPrice);

        // Then
        var snapshot = serviceType.CreateSnapshot();

        var expectedHistory = new[]
        {
            new HealthcareServiceType.PriceHistoryEntry(correctedPrice, testDate, null)
        };

        snapshot.Price.ShouldBe(correctedPrice);
        snapshot.PriceHistory.ShouldBe(expectedHistory);
    }
    
    [Test]
    public void GivenNegativePrice_WhenChangePrice_ThenThrowsArgumentException()
    {
        // Given
        var testDate = new DateOnly(2024, 9, 16);
        ApplicationClock.SetDate(testDate);

        var initialPrice = new Money(100.00m);
        var serviceType = CreateDefaultService(initialPrice);
        var negativePrice = new Money(-50.00m);

        // When
        var exception = Should.Throw<ArgumentException>(() =>
            serviceType.ChangePrice(negativePrice));

        // Then
        exception!.Message.ShouldBe("Service price must be 0 or greater");
    }
    
    private static HealthcareServiceType CreateDefaultService(Money initialPrice)
    {
        return HealthcareServiceType.Create(
            "Routine Check-up",
            "RCU",
            TimeSpan.FromMinutes(30),
            initialPrice,
            new List<string>(),
            new List<string>());
    }
}