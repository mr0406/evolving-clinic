using EvolvingClinic.Domain.Doctors;
using EvolvingClinic.Domain.Shared;
using NUnit.Framework;
using Shouldly;

namespace EvolvingClinic.Domain.UnitTests.Doctors;

public class DoctorWorkScheduleCreateTests : TestBase
{
    [Test]
    public void GivenValidDoctorCodeAndWorkingDays_WhenCreate_ThenCreatesSuccessfully()
    {
        // Given
        var workingDays = new List<DoctorWorkSchedule.WorkingDay>
        {
            new(DayOfWeek.Monday, new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0))),
            new(DayOfWeek.Tuesday, new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0)))
        };

        // When
        var schedule = DoctorWorkSchedule.Create("SMITH", workingDays);

        // Then
        var snapshot = schedule.CreateSnapshot();
        snapshot.DoctorCode.ShouldBe("SMITH");
        snapshot.WeeklySchedule.Count.ShouldBe(2);
        snapshot.WeeklySchedule[DayOfWeek.Monday].Start.ShouldBe(new TimeOnly(9, 0));
        snapshot.WeeklySchedule[DayOfWeek.Monday].End.ShouldBe(new TimeOnly(17, 0));
        snapshot.WeeklySchedule[DayOfWeek.Tuesday].Start.ShouldBe(new TimeOnly(9, 0));
        snapshot.WeeklySchedule[DayOfWeek.Tuesday].End.ShouldBe(new TimeOnly(17, 0));
    }

    [Test]
    public void GivenSingleWorkingDay_WhenCreate_ThenCreatesSuccessfully()
    {
        // Given
        var workingDays = new List<DoctorWorkSchedule.WorkingDay>
        {
            new(DayOfWeek.Saturday, new TimeRange(new TimeOnly(10, 0), new TimeOnly(16, 0)))
        };

        // When
        var schedule = DoctorWorkSchedule.Create("MARTINEZ", workingDays);

        // Then
        var snapshot = schedule.CreateSnapshot();
        snapshot.DoctorCode.ShouldBe("MARTINEZ");
        snapshot.WeeklySchedule.Count.ShouldBe(1);
        snapshot.WeeklySchedule[DayOfWeek.Saturday].Start.ShouldBe(new TimeOnly(10, 0));
        snapshot.WeeklySchedule[DayOfWeek.Saturday].End.ShouldBe(new TimeOnly(16, 0));
    }

    [Test]
    public void GivenAllSevenDays_WhenCreate_ThenCreatesSuccessfully()
    {
        // Given
        var workingDays = new List<DoctorWorkSchedule.WorkingDay>
        {
            new(DayOfWeek.Monday, new TimeRange(new TimeOnly(8, 0), new TimeOnly(16, 0))),
            new(DayOfWeek.Tuesday, new TimeRange(new TimeOnly(8, 0), new TimeOnly(16, 0))),
            new(DayOfWeek.Wednesday, new TimeRange(new TimeOnly(8, 0), new TimeOnly(16, 0))),
            new(DayOfWeek.Thursday, new TimeRange(new TimeOnly(8, 0), new TimeOnly(16, 0))),
            new(DayOfWeek.Friday, new TimeRange(new TimeOnly(8, 0), new TimeOnly(16, 0))),
            new(DayOfWeek.Saturday, new TimeRange(new TimeOnly(8, 0), new TimeOnly(16, 0))),
            new(DayOfWeek.Sunday, new TimeRange(new TimeOnly(8, 0), new TimeOnly(16, 0)))
        };

        // When
        var schedule = DoctorWorkSchedule.Create("WORKAHOLIC", workingDays);

        // Then
        var snapshot = schedule.CreateSnapshot();
        snapshot.WeeklySchedule.Count.ShouldBe(7);
        foreach (DayOfWeek day in Enum.GetValues<DayOfWeek>())
        {
            snapshot.WeeklySchedule[day].Start.ShouldBe(new TimeOnly(8, 0));
            snapshot.WeeklySchedule[day].End.ShouldBe(new TimeOnly(16, 0));
        }
    }

    [Test]
    public void GivenNullDoctorCode_WhenCreate_ThenThrowsArgumentException()
    {
        // Given
        var workingDays = new List<DoctorWorkSchedule.WorkingDay>
        {
            new(DayOfWeek.Monday, new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0)))
        };

        // When
        var exception = Should.Throw<ArgumentException>(() => DoctorWorkSchedule.Create(null!, workingDays));

        // Then
        exception.Message.ShouldBe("Doctor code is required");
    }

    [Test]
    public void GivenEmptyDoctorCode_WhenCreate_ThenThrowsArgumentException()
    {
        // Given
        var workingDays = new List<DoctorWorkSchedule.WorkingDay>
        {
            new(DayOfWeek.Monday, new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0)))
        };

        // When
        var exception = Should.Throw<ArgumentException>(() => DoctorWorkSchedule.Create("", workingDays));

        // Then
        exception.Message.ShouldBe("Doctor code is required");
    }

    [Test]
    public void GivenWhitespaceDoctorCode_WhenCreate_ThenThrowsArgumentException()
    {
        // Given
        var workingDays = new List<DoctorWorkSchedule.WorkingDay>
        {
            new(DayOfWeek.Monday, new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0)))
        };

        // When
        var exception = Should.Throw<ArgumentException>(() => DoctorWorkSchedule.Create("   ", workingDays));

        // Then
        exception.Message.ShouldBe("Doctor code is required");
    }

    [Test]
    public void GivenEmptyWorkingDaysList_WhenCreate_ThenThrowsArgumentException()
    {
        // Given
        var workingDays = new List<DoctorWorkSchedule.WorkingDay>();

        // When
        var exception = Should.Throw<ArgumentException>(() => DoctorWorkSchedule.Create("SMITH", workingDays));

        // Then
        exception.Message.ShouldBe("At least one working day is required");
    }

    [Test]
    public void GivenDuplicateWorkingDays_WhenCreate_ThenThrowsArgumentException()
    {
        // Given
        var workingDays = new List<DoctorWorkSchedule.WorkingDay>
        {
            new(DayOfWeek.Monday, new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0))),
            new(DayOfWeek.Monday, new TimeRange(new TimeOnly(10, 0), new TimeOnly(18, 0)))
        };

        // When
        var exception = Should.Throw<ArgumentException>(() => DoctorWorkSchedule.Create("SMITH", workingDays));

        // Then
        exception.Message.ShouldBe("Duplicate days found: Monday");
    }

    [Test]
    public void GivenMultipleDuplicateWorkingDays_WhenCreate_ThenThrowsArgumentException()
    {
        // Given
        var workingDays = new List<DoctorWorkSchedule.WorkingDay>
        {
            new(DayOfWeek.Monday, new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0))),
            new(DayOfWeek.Monday, new TimeRange(new TimeOnly(10, 0), new TimeOnly(18, 0))),
            new(DayOfWeek.Tuesday, new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0))),
            new(DayOfWeek.Tuesday, new TimeRange(new TimeOnly(11, 0), new TimeOnly(19, 0)))
        };

        // When
        var exception = Should.Throw<ArgumentException>(() => DoctorWorkSchedule.Create("SMITH", workingDays));

        // ThenGood
        exception.Message.ShouldBe("Duplicate days found: Monday, Tuesday");
    }
}