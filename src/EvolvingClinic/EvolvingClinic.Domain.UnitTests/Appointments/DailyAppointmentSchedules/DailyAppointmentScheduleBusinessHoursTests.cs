using EvolvingClinic.Domain.Appointments;
using EvolvingClinic.Domain.Shared;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Appointments.DailyAppointmentSchedules;

public class DailyAppointmentScheduleBusinessHoursTests : TestBase
{
    [Test]
    public void GivenWorkingHours9To17_WhenScheduleAppointmentWithin_ThenIsScheduledSuccessfully()
    {
        // Given
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = CreateScheduleWithWorkingHours(DayOfWeek.Monday, workingHours);

        // When
        var appointment = schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeRange(new TimeOnly(10, 0), new TimeOnly(11, 0)), new Money(100.00m));

        // Then
        appointment.ShouldNotBeNull();
        schedule.CreateSnapshot().Appointments.Count.ShouldBe(1);
    }

    [Test]
    public void GivenWorkingHours9To17_WhenScheduleAppointmentStartingBefore9_ThenThrowsArgumentException()
    {
        // Given
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = CreateScheduleWithWorkingHours(DayOfWeek.Monday, workingHours);

        // When
        Action scheduleAppointment = () =>
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeRange(new TimeOnly(8, 59), new TimeOnly(9, 20)), new Money(100.00m));

        // Then
        var exception = Should.Throw<ArgumentException>(scheduleAppointment);
        exception.Message.ShouldBe("Appointments can only be scheduled between 09:00 and 17:00");
        schedule.CreateSnapshot().Appointments.ShouldBeEmpty();
    }

    [Test]
    public void GivenWorkingHours9To17_WhenScheduleAppointmentEndingAfter17_ThenThrowsArgumentException()
    {
        // Given
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = CreateScheduleWithWorkingHours(DayOfWeek.Monday, workingHours);

        // When
        Action scheduleAppointment = () =>
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeRange(new TimeOnly(16, 50), new TimeOnly(17, 10)), new Money(100.00m));

        // Then
        var exception = Should.Throw<ArgumentException>(scheduleAppointment);
        exception.Message.ShouldBe("Appointments can only be scheduled between 09:00 and 17:00");
        schedule.CreateSnapshot().Appointments.ShouldBeEmpty();
    }

    [Test]
    public void GivenWorkingHours8To18_WhenScheduleAppointmentAt7_ThenThrowsArgumentException()
    {
        // Given
        var workingHours = new TimeRange(new TimeOnly(8, 0), new TimeOnly(18, 0));
        var schedule = CreateScheduleWithWorkingHours(DayOfWeek.Tuesday, workingHours);

        // When
        Action scheduleAppointment = () =>
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeRange(new TimeOnly(7, 30), new TimeOnly(8, 30)), new Money(100.00m));

        // Then
        var exception = Should.Throw<ArgumentException>(scheduleAppointment);
        exception.Message.ShouldBe("Appointments can only be scheduled between 08:00 and 18:00");
    }

    [Test]
    public void GivenWorkingHours10To16_WhenScheduleAppointmentExactlyAt10To16_ThenIsScheduledSuccessfully()
    {
        // Given
        var workingHours = new TimeRange(new TimeOnly(10, 0), new TimeOnly(16, 0));
        var schedule = CreateScheduleWithWorkingHours(DayOfWeek.Saturday, workingHours);

        // When
        var appointment = schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeRange(new TimeOnly(10, 0), new TimeOnly(16, 0)), new Money(100.00m));

        // Then
        appointment.ShouldNotBeNull();
        schedule.CreateSnapshot().Appointments.Count.ShouldBe(1);
    }

    [Test]
    public void GivenDifferentWorkingHours_WhenScheduleAppointment_ThenValidatesAgainstSpecificHours()
    {
        // Given
        var morningHours = new TimeRange(new TimeOnly(6, 0), new TimeOnly(12, 0));
        var schedule = CreateScheduleWithWorkingHours(DayOfWeek.Sunday, morningHours);

        // When
        var appointment = schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeRange(new TimeOnly(8, 0), new TimeOnly(10, 0)), new Money(100.00m));

        // Then
        appointment.ShouldNotBeNull();
        schedule.CreateSnapshot().Appointments.Count.ShouldBe(1);
    }

    private static DailyAppointmentSchedule CreateScheduleWithWorkingHours(DayOfWeek dayOfWeek, TimeRange workingHours)
    {
        var date = GetDateFor(dayOfWeek);
        var key = new DailyAppointmentSchedule.Key("SMITH", date);
        return DailyAppointmentSchedule.Create(key, workingHours);
    }

    private static DateOnly GetDateFor(DayOfWeek targetDayOfWeek)
    {
        var monday = new DateOnly(2025, 9, 1);
        var daysToAdd = (int)targetDayOfWeek - (int)DayOfWeek.Monday;

        if (daysToAdd < 0) {
            daysToAdd += 7; // Handle Sunday
        }

        return monday.AddDays(daysToAdd);
    }
}