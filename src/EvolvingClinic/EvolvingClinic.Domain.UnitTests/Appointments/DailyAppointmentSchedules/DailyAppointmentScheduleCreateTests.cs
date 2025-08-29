using EvolvingClinic.Domain.Appointments;
using FluentAssertions;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Appointments.DailyAppointmentSchedules;

[TestFixture]
public class DailyAppointmentScheduleCreateTests
{
    [Test]
    public void GivenValidDate_WhenCreateDailyAppointmentSchedule_ThenIsCreatedSuccessfully()
    {
        // Given
        var scheduleDate = new DateOnly(2024, 1, 15);

        // When
        var schedule = new DailyAppointmentSchedule(scheduleDate);

        // Then
        schedule.Should().NotBeNull();
        schedule.Date.Should().Be(scheduleDate);
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
        snapshot.Date.Should().Be(scheduleDate);
        snapshot.Appointments.Should().BeEmpty();
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
        firstSchedule.Date.Should().Be(firstDate);
        secondSchedule.Date.Should().Be(secondDate);
        thirdSchedule.Date.Should().Be(thirdDate);

        var firstSnapshot = firstSchedule.CreateSnapshot();
        var secondSnapshot = secondSchedule.CreateSnapshot();
        var thirdSnapshot = thirdSchedule.CreateSnapshot();

        firstSnapshot.Date.Should().Be(firstDate);
        secondSnapshot.Date.Should().Be(secondDate);
        thirdSnapshot.Date.Should().Be(thirdDate);

        firstSnapshot.Appointments.Should().BeEmpty();
        secondSnapshot.Appointments.Should().BeEmpty();
        thirdSnapshot.Appointments.Should().BeEmpty();
    }

    [Test]
    public void GivenLeapYearDate_WhenCreateDailyAppointmentSchedule_ThenIsCreatedSuccessfully()
    {
        // Given
        var leapYearDate = new DateOnly(2024, 2, 29); // 2024 is a leap year

        // When
        var schedule = new DailyAppointmentSchedule(leapYearDate);

        // Then
        schedule.Date.Should().Be(leapYearDate);
        var snapshot = schedule.CreateSnapshot();
        snapshot.Date.Should().Be(leapYearDate);
        snapshot.Appointments.Should().BeEmpty();
    }

    [Test]
    public void GivenMinimumDate_WhenCreateDailyAppointmentSchedule_ThenIsCreatedSuccessfully()
    {
        // Given
        var minimumDate = DateOnly.MinValue;

        // When
        var schedule = new DailyAppointmentSchedule(minimumDate);

        // Then
        schedule.Date.Should().Be(minimumDate);
        var snapshot = schedule.CreateSnapshot();
        snapshot.Date.Should().Be(minimumDate);
        snapshot.Appointments.Should().BeEmpty();
    }

    [Test]
    public void GivenMaximumDate_WhenCreateDailyAppointmentSchedule_ThenIsCreatedSuccessfully()
    {
        // Given
        var maximumDate = DateOnly.MaxValue;

        // When
        var schedule = new DailyAppointmentSchedule(maximumDate);

        // Then
        schedule.Date.Should().Be(maximumDate);
        var snapshot = schedule.CreateSnapshot();
        snapshot.Date.Should().Be(maximumDate);
        snapshot.Appointments.Should().BeEmpty();
    }
}