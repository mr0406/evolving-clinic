using EvolvingClinic.Domain.Appointments;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Appointments.DailyAppointmentSchedules;

public class DailyAppointmentScheduleCreateTests : TestBase
{
    [Test]
    public void GivenValidDate_WhenCreateDailyAppointmentSchedule_ThenIsCreatedSuccessfully()
    {
        // Given
        var scheduleDate = new DateOnly(2024, 1, 15);

        // When
        var schedule = new DailyAppointmentSchedule(scheduleDate);

        // Then
        schedule.ShouldNotBeNull();
        schedule.Date.ShouldBe(scheduleDate);
    }

    [Test]
    public void GivenValidDate_WhenCreateDailyAppointmentSchedule_ThenHasNoAppointments()
    {
        // Given
        var scheduleDate = new DateOnly(2024, 1, 15);

        // When
        var schedule = new DailyAppointmentSchedule(scheduleDate);

        // Then
        var snapshot = schedule.CreateSnapshot();
        snapshot.Date.ShouldBe(scheduleDate);
        snapshot.Appointments.ShouldBeEmpty();
    }

    [Test]
    public void GivenDifferentDates_WhenCreateMultipleDailyAppointmentSchedules_ThenEachHasCorrectDate()
    {
        // Given
        var firstDate = new DateOnly(2024, 1, 15);
        var secondDate = new DateOnly(2024, 1, 16);
        var thirdDate = new DateOnly(2024, 2, 1);

        // When
        var firstSchedule = new DailyAppointmentSchedule(firstDate);
        var secondSchedule = new DailyAppointmentSchedule(secondDate);
        var thirdSchedule = new DailyAppointmentSchedule(thirdDate);

        // Then
        firstSchedule.Date.ShouldBe(firstDate);
        secondSchedule.Date.ShouldBe(secondDate);
        thirdSchedule.Date.ShouldBe(thirdDate);

        var firstSnapshot = firstSchedule.CreateSnapshot();
        var secondSnapshot = secondSchedule.CreateSnapshot();
        var thirdSnapshot = thirdSchedule.CreateSnapshot();

        firstSnapshot.Date.ShouldBe(firstDate);
        secondSnapshot.Date.ShouldBe(secondDate);
        thirdSnapshot.Date.ShouldBe(thirdDate);

        firstSnapshot.Appointments.ShouldBeEmpty();
        secondSnapshot.Appointments.ShouldBeEmpty();
        thirdSnapshot.Appointments.ShouldBeEmpty();
    }

    [Test]
    public void GivenLeapYearDate_WhenCreateDailyAppointmentSchedule_ThenIsCreatedSuccessfully()
    {
        // Given
        var leapYearDate = new DateOnly(2024, 2, 29); // 2024 is a leap year

        // When
        var schedule = new DailyAppointmentSchedule(leapYearDate);

        // Then
        schedule.Date.ShouldBe(leapYearDate);
        var snapshot = schedule.CreateSnapshot();
        snapshot.Date.ShouldBe(leapYearDate);
        snapshot.Appointments.ShouldBeEmpty();
    }

    [Test]
    public void GivenMinimumDate_WhenCreateDailyAppointmentSchedule_ThenIsCreatedSuccessfully()
    {
        // Given
        var minimumDate = DateOnly.MinValue;

        // When
        var schedule = new DailyAppointmentSchedule(minimumDate);

        // Then
        schedule.Date.ShouldBe(minimumDate);
        var snapshot = schedule.CreateSnapshot();
        snapshot.Date.ShouldBe(minimumDate);
        snapshot.Appointments.ShouldBeEmpty();
    }

    [Test]
    public void GivenMaximumDate_WhenCreateDailyAppointmentSchedule_ThenIsCreatedSuccessfully()
    {
        // Given
        var maximumDate = DateOnly.MaxValue;

        // When
        var schedule = new DailyAppointmentSchedule(maximumDate);

        // Then
        schedule.Date.ShouldBe(maximumDate);
        var snapshot = schedule.CreateSnapshot();
        snapshot.Date.ShouldBe(maximumDate);
        snapshot.Appointments.ShouldBeEmpty();
    }
}