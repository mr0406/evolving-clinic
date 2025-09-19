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
        var workingHours = new TimeRange(new TimeOnly(7, 0), new TimeOnly(16, 0));
        var schedule = DailyAppointmentSchedule.Create(key, workingHours);

        // Then
        schedule.ShouldNotBeNull();
        schedule.ScheduleKey.ShouldBe(key);
        schedule.ScheduleKey.DoctorCode.ShouldBe(doctorCode);
        schedule.ScheduleKey.Date.ShouldBe(scheduleDate);

        var snapshot = schedule.CreateSnapshot();
        snapshot.Date.ShouldBe(scheduleDate);
        snapshot.Appointments.ShouldBeEmpty();
        snapshot.WorkingHours.ShouldBe(workingHours);
    }
}