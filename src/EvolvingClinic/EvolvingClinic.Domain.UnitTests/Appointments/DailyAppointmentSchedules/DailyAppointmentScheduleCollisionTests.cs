using EvolvingClinic.Domain.Appointments;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Appointments.DailyAppointmentSchedules;

[TestFixture]
public class DailyAppointmentScheduleCollisionTests
{
    private readonly DateOnly _scheduleDate = new(2024, 1, 15);

    [Test]
    public void GivenScheduleWithAppointment_WhenScheduleOverlappingAppointment_ThenThrowsArgumentException()
    {
        // Given
        var schedule = new DailyAppointmentSchedule(_scheduleDate);
        schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(10, 0), new TimeOnly(11, 0));
        
        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(10, 30), new TimeOnly(11, 30)));
        
        // Then
        exception!.Message.ShouldBe("Appointment time slot conflicts with existing appointment");

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Count.ShouldBe(1);
    }

    [Test]
    public void GivenScheduleWithAppointment_WhenScheduleExactlyOverlappingAppointment_ThenThrowsArgumentException()
    {
        // Given
        var schedule = new DailyAppointmentSchedule(_scheduleDate);
        schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(10, 0), new TimeOnly(11, 0));
        
        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(10, 0), new TimeOnly(11, 0)));
        
        // Then
        exception!.Message.ShouldBe("Appointment time slot conflicts with existing appointment");

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Count.ShouldBe(1);
    }

    [Test]
    public void GivenScheduleWithAppointment_WhenScheduleAppointmentStartingDuringExisting_ThenThrowsArgumentException()
    {
        // Given
        var schedule = new DailyAppointmentSchedule(_scheduleDate);
        schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(10, 0), new TimeOnly(11, 0));
        
        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(10, 45), new TimeOnly(12, 0)));
        
        // Then
        exception!.Message.ShouldBe("Appointment time slot conflicts with existing appointment");

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Count.ShouldBe(1);
    }

    [Test]
    public void GivenScheduleWithAppointment_WhenScheduleAppointmentEndingDuringExisting_ThenThrowsArgumentException()
    {
        // Given
        var schedule = new DailyAppointmentSchedule(_scheduleDate);
        schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(10, 0), new TimeOnly(11, 0));
        
        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(9, 0), new TimeOnly(10, 30)));
        
        // Then
        exception!.Message.ShouldBe("Appointment time slot conflicts with existing appointment");

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Count.ShouldBe(1);
    }

    [Test]
    public void GivenScheduleWithAppointment_WhenScheduleAppointmentCompletelyEnclosingExisting_ThenThrowsArgumentException()
    {
        // Given
        var schedule = new DailyAppointmentSchedule(_scheduleDate);
        schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(10, 0), new TimeOnly(11, 0));
        
        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(9, 0), new TimeOnly(12, 0)));
        
        // Then
        exception!.Message.ShouldBe("Appointment time slot conflicts with existing appointment");

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Count.ShouldBe(1);
    }

    [Test]
    public void GivenScheduleWithAppointment_WhenScheduleAdjacentAppointmentBefore_ThenBothAreScheduled()
    {
        // Given
        var schedule = new DailyAppointmentSchedule(_scheduleDate);
        var firstAppointment = schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(10, 0), new TimeOnly(11, 0));
        
        // When
        var secondAppointment = schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(9, 0), new TimeOnly(10, 0));

        // Then
        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Count.ShouldBe(2);
        
        var firstSnapshot = snapshot.Appointments.First(a => a.Id == firstAppointment.Id);
        firstSnapshot.StartTime.ShouldBe(_scheduleDate.ToDateTime(new TimeOnly(10, 0)));
        firstSnapshot.EndTime.ShouldBe(_scheduleDate.ToDateTime(new TimeOnly(11, 0)));
        
        var secondSnapshot = snapshot.Appointments.First(a => a.Id == secondAppointment.Id);
        secondSnapshot.StartTime.ShouldBe(_scheduleDate.ToDateTime(new TimeOnly(9, 0)));
        secondSnapshot.EndTime.ShouldBe(_scheduleDate.ToDateTime(new TimeOnly(10, 0)));
    }

    [Test]
    public void GivenScheduleWithAppointment_WhenScheduleAdjacentAppointmentAfter_ThenBothAreScheduled()
    {
        // Given
        var schedule = new DailyAppointmentSchedule(_scheduleDate);
        var firstAppointment = schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(10, 0), new TimeOnly(11, 0));
        
        // When
        var secondAppointment = schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(11, 0), new TimeOnly(12, 0));

        // Then
        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Count.ShouldBe(2);
        
        var firstSnapshot = snapshot.Appointments.First(a => a.Id == firstAppointment.Id);
        firstSnapshot.StartTime.ShouldBe(_scheduleDate.ToDateTime(new TimeOnly(10, 0)));
        firstSnapshot.EndTime.ShouldBe(_scheduleDate.ToDateTime(new TimeOnly(11, 0)));
        
        var secondSnapshot = snapshot.Appointments.First(a => a.Id == secondAppointment.Id);
        secondSnapshot.StartTime.ShouldBe(_scheduleDate.ToDateTime(new TimeOnly(11, 0)));
        secondSnapshot.EndTime.ShouldBe(_scheduleDate.ToDateTime(new TimeOnly(12, 0)));
    }

    [Test]
    public void GivenScheduleWithMultipleAppointments_WhenScheduleConflictingAppointment_ThenThrowsArgumentException()
    {
        // Given
        var schedule = new DailyAppointmentSchedule(_scheduleDate);
        
        schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(9, 0), new TimeOnly(10, 0));
        schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(11, 0), new TimeOnly(12, 0));
        
        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeOnly(9, 30), new TimeOnly(10, 30)));
        
        // Then
        exception!.Message.ShouldBe("Appointment time slot conflicts with existing appointment");

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Count.ShouldBe(2);
    }
}