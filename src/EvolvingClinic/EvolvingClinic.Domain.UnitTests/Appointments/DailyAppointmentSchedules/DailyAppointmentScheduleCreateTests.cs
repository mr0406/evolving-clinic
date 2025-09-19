using EvolvingClinic.Domain.Appointments;
using EvolvingClinic.Domain.Shared;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Appointments.DailyAppointmentSchedules;

public class DailyAppointmentScheduleCreateTests : TestBase
{
    [Test]
    public void GivenValidKey_WhenCreateDailyAppointmentSchedule_ThenIsCreatedSuccessfully()
    {
        // Given
        var doctorCode = "SMITH";
        var scheduleDate = new DateOnly(2024, 1, 15);
        var key = new DailyAppointmentSchedule.Key(doctorCode, scheduleDate);

        // When
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = DailyAppointmentSchedule.Create(key, workingHours);

        // Then
        schedule.ShouldNotBeNull();
        schedule.ScheduleKey.ShouldBe(key);
        schedule.ScheduleKey.DoctorCode.ShouldBe(doctorCode);
        schedule.ScheduleKey.Date.ShouldBe(scheduleDate);
    }

    [Test]
    public void GivenValidKey_WhenCreateDailyAppointmentSchedule_ThenHasNoAppointments()
    {
        // Given
        var doctorCode = "SMITH";
        var scheduleDate = new DateOnly(2024, 1, 15);
        var key = new DailyAppointmentSchedule.Key(doctorCode, scheduleDate);

        // When
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = DailyAppointmentSchedule.Create(key, workingHours);

        // Then
        var snapshot = schedule.CreateSnapshot();
        snapshot.DoctorCode.ShouldBe(doctorCode);
        snapshot.Date.ShouldBe(scheduleDate);
        snapshot.Appointments.ShouldBeEmpty();
    }

    [Test]
    public void GivenDifferentKeys_WhenCreateMultipleDailyAppointmentSchedules_ThenEachHasCorrectKey()
    {
        // Given
        var firstKey = new DailyAppointmentSchedule.Key("SMITH", new DateOnly(2024, 1, 15));
        var secondKey = new DailyAppointmentSchedule.Key("JONES", new DateOnly(2024, 1, 16));
        var thirdKey = new DailyAppointmentSchedule.Key("SMITH", new DateOnly(2024, 2, 1));

        // When
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var firstSchedule = DailyAppointmentSchedule.Create(firstKey, workingHours);
        var secondSchedule = DailyAppointmentSchedule.Create(secondKey, workingHours);
        var thirdSchedule = DailyAppointmentSchedule.Create(thirdKey, workingHours);

        // Then
        firstSchedule.ScheduleKey.ShouldBe(firstKey);
        secondSchedule.ScheduleKey.ShouldBe(secondKey);
        thirdSchedule.ScheduleKey.ShouldBe(thirdKey);

        var firstSnapshot = firstSchedule.CreateSnapshot();
        var secondSnapshot = secondSchedule.CreateSnapshot();
        var thirdSnapshot = thirdSchedule.CreateSnapshot();

        firstSnapshot.DoctorCode.ShouldBe("SMITH");
        firstSnapshot.Date.ShouldBe(new DateOnly(2024, 1, 15));
        secondSnapshot.DoctorCode.ShouldBe("JONES");
        secondSnapshot.Date.ShouldBe(new DateOnly(2024, 1, 16));
        thirdSnapshot.DoctorCode.ShouldBe("SMITH");
        thirdSnapshot.Date.ShouldBe(new DateOnly(2024, 2, 1));

        firstSnapshot.Appointments.ShouldBeEmpty();
        secondSnapshot.Appointments.ShouldBeEmpty();
        thirdSnapshot.Appointments.ShouldBeEmpty();
    }

    [Test]
    public void GivenLeapYearDate_WhenCreateDailyAppointmentSchedule_ThenIsCreatedSuccessfully()
    {
        // Given
        var leapYearDate = new DateOnly(2024, 2, 29); // 2024 is a leap year
        var key = new DailyAppointmentSchedule.Key("SMITH", leapYearDate);

        // When
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = DailyAppointmentSchedule.Create(key, workingHours);

        // Then
        schedule.ScheduleKey.Date.ShouldBe(leapYearDate);
        var snapshot = schedule.CreateSnapshot();
        snapshot.Date.ShouldBe(leapYearDate);
        snapshot.Appointments.ShouldBeEmpty();
    }

    [Test]
    public void GivenMinimumDate_WhenCreateDailyAppointmentSchedule_ThenIsCreatedSuccessfully()
    {
        // Given
        var minimumDate = DateOnly.MinValue;
        var key = new DailyAppointmentSchedule.Key("SMITH", minimumDate);

        // When
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = DailyAppointmentSchedule.Create(key, workingHours);

        // Then
        schedule.ScheduleKey.Date.ShouldBe(minimumDate);
        var snapshot = schedule.CreateSnapshot();
        snapshot.Date.ShouldBe(minimumDate);
        snapshot.Appointments.ShouldBeEmpty();
    }

    [Test]
    public void GivenMaximumDate_WhenCreateDailyAppointmentSchedule_ThenIsCreatedSuccessfully()
    {
        // Given
        var maximumDate = DateOnly.MaxValue;
        var key = new DailyAppointmentSchedule.Key("SMITH", maximumDate);

        // When
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = DailyAppointmentSchedule.Create(key, workingHours);

        // Then
        schedule.ScheduleKey.Date.ShouldBe(maximumDate);
        var snapshot = schedule.CreateSnapshot();
        snapshot.Date.ShouldBe(maximumDate);
        snapshot.Appointments.ShouldBeEmpty();
    }
}