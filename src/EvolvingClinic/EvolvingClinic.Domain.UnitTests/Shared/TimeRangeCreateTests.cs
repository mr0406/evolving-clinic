using EvolvingClinic.Domain.Shared;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Shared;

public class TimeRangeCreateTests : TestBase
{
    [Test]
    public void GivenValidStartAndEndTimes_WhenCreateTimeRange_ThenIsCreatedSuccessfully()
    {
        // Given
        var start = new TimeOnly(9, 0);
        var end = new TimeOnly(17, 0);

        // When
        var timeRange = new TimeRange(start, end);

        // Then
        timeRange.Start.ShouldBe(start);
        timeRange.End.ShouldBe(end);
    }

    [Test]
    public void GivenStartTimeEqualToEndTime_WhenCreateTimeRange_ThenThrowsArgumentException()
    {
        // Given
        var time = new TimeOnly(12, 0);

        // When
        var exception = Should.Throw<ArgumentException>(() => new TimeRange(time, time));

        // Then
        exception.Message.ShouldBe("End time must be after start time");
    }

    [Test]
    public void GivenEndTimeBeforeStartTime_WhenCreateTimeRange_ThenThrowsArgumentException()
    {
        // Given
        var start = new TimeOnly(17, 0);
        var end = new TimeOnly(9, 0);

        // When
        var exception = Should.Throw<ArgumentException>(() => new TimeRange(start, end));

        // Then
        exception.Message.ShouldBe("End time must be after start time");
    }

    [Test]
    public void GivenMinimalTimeDifference_WhenCreateTimeRange_ThenIsCreatedSuccessfully()
    {
        // Given
        var start = new TimeOnly(9, 0);
        var end = new TimeOnly(9, 1);

        // When
        var timeRange = new TimeRange(start, end);

        // Then
        timeRange.Start.ShouldBe(start);
        timeRange.End.ShouldBe(end);
    }

    [Test]
    public void GivenFullDayRange_WhenCreateTimeRange_ThenIsCreatedSuccessfully()
    {
        // Given
        var start = TimeOnly.MinValue;
        var end = TimeOnly.MaxValue;

        // When
        var timeRange = new TimeRange(start, end);

        // Then
        timeRange.Start.ShouldBe(start);
        timeRange.End.ShouldBe(end);
    }

    [Test]
    public void GivenMidnightToOneMinuteBeforeMidnight_WhenCreateTimeRange_ThenIsCreatedSuccessfully()
    {
        // Given
        var start = new TimeOnly(0, 0);
        var end = new TimeOnly(23, 59);

        // When
        var timeRange = new TimeRange(start, end);

        // Then
        timeRange.Start.ShouldBe(start);
        timeRange.End.ShouldBe(end);
    }
}