using EvolvingClinic.Domain.Appointments;
using FluentAssertions;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Appointments.AppoitmentTimeSlots;

[TestFixture]
public class AppointmentTimeSlotCreateTests
{
    [Test]
    public void GivenValidDateAndTimes_WhenCreateAppointmentTimeSlot_ThenIsCreatedSuccessfully()
    {
        // Given
        var date = new DateOnly(2024, 1, 15);
        var startTime = new TimeOnly(10, 0);
        var endTime = new TimeOnly(11, 0);

        // When
        var timeSlot = new AppointmentTimeSlot(date, startTime, endTime);

        // Then
        timeSlot.Date.Should().Be(date);
        timeSlot.StartTime.Should().Be(startTime);
        timeSlot.EndTime.Should().Be(endTime);
        timeSlot.Duration.Should().Be(TimeSpan.FromHours(1));
        timeSlot.StartDateTime.Should().Be(date.ToDateTime(startTime));
        timeSlot.EndDateTime.Should().Be(date.ToDateTime(endTime));
    }

    [Test]
    public void GivenExactly15MinuteDuration_WhenCreateAppointmentTimeSlot_ThenIsCreatedSuccessfully()
    {
        // Given
        var date = new DateOnly(2024, 1, 15);
        var startTime = new TimeOnly(10, 0);
        var endTime = new TimeOnly(10, 15);

        // When
        var timeSlot = new AppointmentTimeSlot(date, startTime, endTime);

        // Then
        timeSlot.Duration.Should().Be(TimeSpan.FromMinutes(15));
    }

    [Test]
    public void GivenStartTimeAfterEndTime_WhenCreateAppointmentTimeSlot_ThenThrowsArgumentException()
    {
        // Given
        var date = new DateOnly(2024, 1, 15);
        var startTime = new TimeOnly(11, 0);
        var endTime = new TimeOnly(10, 0);

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            new AppointmentTimeSlot(date, startTime, endTime));
        
        // Then
        exception!.Message.Should().Be("Start time must be before end time");
    }

    [Test]
    public void GivenStartTimeEqualToEndTime_WhenCreateAppointmentTimeSlot_ThenThrowsArgumentException()
    {
        // Given
        var date = new DateOnly(2024, 1, 15);
        var sameTime = new TimeOnly(10, 0);

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            new AppointmentTimeSlot(date, sameTime, sameTime));
        
        // Then
        exception!.Message.Should().Be("Start time must be before end time");
    }

    [Test]
    public void GivenDurationLessThan15Minutes_WhenCreateAppointmentTimeSlot_ThenThrowsArgumentException()
    {
        // Given
        var date = new DateOnly(2024, 1, 15);
        var startTime = new TimeOnly(10, 0);
        var endTime = new TimeOnly(10, 10);

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            new AppointmentTimeSlot(date, startTime, endTime));
        
        // Then
        exception!.Message.Should().Be("Appointment must be at least 15 minutes long");
    }
}