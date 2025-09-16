using EvolvingClinic.Domain.Appointments;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Appointments.AppointmentTimeSlots;

public class AppointmentTimeSlotCreateTests : TestBase
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
        timeSlot.Date.ShouldBe(date);
        timeSlot.StartTime.ShouldBe(startTime);
        timeSlot.EndTime.ShouldBe(endTime);
        timeSlot.Duration.ShouldBe(TimeSpan.FromHours(1));
        timeSlot.StartDateTime.ShouldBe(date.ToDateTime(startTime));
        timeSlot.EndDateTime.ShouldBe(date.ToDateTime(endTime));
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
        timeSlot.Duration.ShouldBe(TimeSpan.FromMinutes(15));
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
        exception!.Message.ShouldBe("Start time must be before end time");
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
        exception!.Message.ShouldBe("Start time must be before end time");
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
        exception!.Message.ShouldBe("Appointment must be at least 15 minutes long");
    }
}