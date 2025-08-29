using EvolvingClinic.Domain.Appointments;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Appointments.AppointmentTimeSlots;

[TestFixture]
public class AppointmentTimeSlotOverlapsWithTests
{
    private readonly DateOnly _testDate = new(2024, 1, 15);

    [Test]
    public void GivenTwoNonOverlappingTimeSlots_WhenCheckOverlapsWith_ThenReturnsFalse()
    {
        // Given
        var firstTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(9, 0), new TimeOnly(10, 0));
        var secondTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(11, 0), new TimeOnly(12, 0));

        // When
        var overlaps = firstTimeSlot.OverlapsWith(secondTimeSlot);

        // Then
        overlaps.ShouldBeFalse();
    }

    [Test]
    public void GivenAdjacentTimeSlots_WhenCheckOverlapsWith_ThenReturnsFalse()
    {
        // Given
        var firstTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(10, 0), new TimeOnly(11, 0));
        var secondTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(11, 0), new TimeOnly(12, 0));

        // When
        var overlaps = firstTimeSlot.OverlapsWith(secondTimeSlot);

        // Then
        overlaps.ShouldBeFalse();
    }

    [Test]
    public void GivenOverlappingTimeSlots_WhenCheckOverlapsWith_ThenReturnsTrue()
    {
        // Given
        var firstTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(10, 0), new TimeOnly(11, 0));
        var secondTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(10, 30), new TimeOnly(11, 30));

        // When
        var overlaps = firstTimeSlot.OverlapsWith(secondTimeSlot);

        // Then
        overlaps.ShouldBeTrue();
    }

    [Test]
    public void GivenIdenticalTimeSlots_WhenCheckOverlapsWith_ThenReturnsTrue()
    {
        // Given
        var firstTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(10, 0), new TimeOnly(11, 0));
        var secondTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(10, 0), new TimeOnly(11, 0));

        // When
        var overlaps = firstTimeSlot.OverlapsWith(secondTimeSlot);

        // Then
        overlaps.ShouldBeTrue();
    }

    [Test]
    public void GivenTimeSlotStartingDuringAnother_WhenCheckOverlapsWith_ThenReturnsTrue()
    {
        // Given
        var firstTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(10, 0), new TimeOnly(11, 0));
        var secondTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(10, 45), new TimeOnly(12, 0));

        // When
        var overlaps = firstTimeSlot.OverlapsWith(secondTimeSlot);

        // Then
        overlaps.ShouldBeTrue();
    }

    [Test]
    public void GivenTimeSlotEndingDuringAnother_WhenCheckOverlapsWith_ThenReturnsTrue()
    {
        // Given
        var firstTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(10, 0), new TimeOnly(11, 0));
        var secondTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(9, 0), new TimeOnly(10, 30));

        // When
        var overlaps = firstTimeSlot.OverlapsWith(secondTimeSlot);

        // Then
        overlaps.ShouldBeTrue();
    }

    [Test]
    public void GivenTimeSlotCompletelyEnclosingAnother_WhenCheckOverlapsWith_ThenReturnsTrue()
    {
        // Given
        var firstTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(10, 0), new TimeOnly(11, 0));
        var secondTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(9, 0), new TimeOnly(12, 0));

        // When
        var overlaps = firstTimeSlot.OverlapsWith(secondTimeSlot);

        // Then
        overlaps.ShouldBeTrue();
    }

    [Test]
    public void GivenTimeSlotCompletelyWithinAnother_WhenCheckOverlapsWith_ThenReturnsTrue()
    {
        // Given
        var firstTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(9, 0), new TimeOnly(12, 0));
        var secondTimeSlot = new AppointmentTimeSlot(_testDate, new TimeOnly(10, 0), new TimeOnly(11, 0));

        // When
        var overlaps = firstTimeSlot.OverlapsWith(secondTimeSlot);

        // Then
        overlaps.ShouldBeTrue();
    }

    [Test]
    public void GivenTimeSlotsOnDifferentDates_WhenCheckOverlapsWith_ThenReturnsFalse()
    {
        // Given
        var firstDate = new DateOnly(2024, 1, 15);
        var secondDate = new DateOnly(2024, 1, 16);
        var firstTimeSlot = new AppointmentTimeSlot(firstDate, new TimeOnly(10, 0), new TimeOnly(11, 0));
        var secondTimeSlot = new AppointmentTimeSlot(secondDate, new TimeOnly(10, 0), new TimeOnly(11, 0));

        // When
        var overlaps = firstTimeSlot.OverlapsWith(secondTimeSlot);

        // Then
        overlaps.ShouldBeFalse();
    }

    [Test]
    public void GivenOverlappingTimeSlotsOnDifferentDates_WhenCheckOverlapsWith_ThenReturnsFalse()
    {
        // Given
        var firstDate = new DateOnly(2024, 1, 15);
        var secondDate = new DateOnly(2024, 1, 16);
        var firstTimeSlot = new AppointmentTimeSlot(firstDate, new TimeOnly(10, 0), new TimeOnly(11, 0));
        var secondTimeSlot = new AppointmentTimeSlot(secondDate, new TimeOnly(10, 30), new TimeOnly(11, 30));

        // When
        var overlaps = firstTimeSlot.OverlapsWith(secondTimeSlot);

        // Then
        overlaps.ShouldBeFalse();
    }
}