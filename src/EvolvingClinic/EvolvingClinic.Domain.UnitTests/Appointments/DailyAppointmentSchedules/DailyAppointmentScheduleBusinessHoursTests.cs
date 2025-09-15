using EvolvingClinic.Domain.Appointments;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Appointments.DailyAppointmentSchedules;

[TestFixture]
public class DailyAppointmentScheduleBusinessHoursTests
{
    [TestCase(DayOfWeek.Saturday)]
    [TestCase(DayOfWeek.Sunday)]
    public void ShouldRejectWeekendAppointments(DayOfWeek dayOfWeek)
    {
        // Given
        var schedule = CreateScheduleFor(dayOfWeek);

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(10, 0), new TimeOnly(11, 0)));

        // Then
        exception!.Message.ShouldBe("Appointments can only be scheduled Monday through Friday");
        schedule.CreateSnapshot().Appointments.ShouldBeEmpty();
    }

    [TestCase(DayOfWeek.Monday)]
    [TestCase(DayOfWeek.Tuesday)]
    [TestCase(DayOfWeek.Wednesday)]
    [TestCase(DayOfWeek.Thursday)]
    [TestCase(DayOfWeek.Friday)]
    public void ShouldAllowWeekdayAppointmentsDuringBusinessHours(DayOfWeek dayOfWeek)
    {
        // Given
        var schedule = CreateScheduleFor(dayOfWeek);

        // When
        var appointment = schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(10, 0), new TimeOnly(11, 0));

        // Then
        appointment.ShouldNotBeNull();
        schedule.CreateSnapshot().Appointments.Count.ShouldBe(1);
    }
    
    [Test]
    public void ShouldRejectAppointmentsStartingBeforeBusinessHours()
    {
        // Given
        var schedule = CreateScheduleFor(DayOfWeek.Monday);

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(8, 59), new TimeOnly(9, 20)));

        // Then
        exception!.Message.ShouldBe("Appointments can only be scheduled between 9:00 AM and 5:00 PM");
        schedule.CreateSnapshot().Appointments.ShouldBeEmpty();
    }
    
    [Test]
    public void ShouldRejectAppointmentsEndingAfterBusinessHours()
    {
        // Given
        var schedule = CreateScheduleFor(DayOfWeek.Monday);

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(16, 50), new TimeOnly(17, 10)));

        // Then
        exception!.Message.ShouldBe("Appointments can only be scheduled between 9:00 AM and 5:00 PM");
        schedule.CreateSnapshot().Appointments.ShouldBeEmpty();
    }

    [Test]
    public void ShouldAllowAppointmentStartingExactlyAtNine()
    {
        // Given
        var schedule = CreateScheduleFor(DayOfWeek.Monday);

        // When
        var appointment = schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(9, 0), new TimeOnly(10, 0));

        // Then
        appointment.ShouldNotBeNull();
        schedule.CreateSnapshot().Appointments.Count.ShouldBe(1);
    }

    [Test]
    public void ShouldAllowAppointmentEndingExactlyAtFive()
    {
        // Given
        var schedule = CreateScheduleFor(DayOfWeek.Monday);

        // When
        var appointment = schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(16, 0), new TimeOnly(17, 0));

        // Then
        appointment.ShouldNotBeNull();
        schedule.CreateSnapshot().Appointments.Count.ShouldBe(1);
    }
    
    private static DailyAppointmentSchedule CreateScheduleFor(DayOfWeek dayOfWeek) => new(GetDateFor(dayOfWeek));

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