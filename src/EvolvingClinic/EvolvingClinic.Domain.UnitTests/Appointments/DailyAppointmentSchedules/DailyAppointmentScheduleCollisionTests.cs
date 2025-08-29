using EvolvingClinic.Domain.Appointments;
using FluentAssertions;
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
        var firstTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(10, 0), new TimeOnly(11, 0));
        schedule.ScheduleAppointment("Jan Kowalski", firstTimeSlot);
        
        var overlappingTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(10, 30), new TimeOnly(11, 30));

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment("Anna Nowak", overlappingTimeSlot));
        
        // Then
        exception!.Message.Should().Be("Appointment time slot conflicts with existing appointment");

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Should().HaveCount(1);
    }

    [Test]
    public void GivenScheduleWithAppointment_WhenScheduleExactlyOverlappingAppointment_ThenThrowsArgumentException()
    {
        // Given
        var schedule = new DailyAppointmentSchedule(_scheduleDate);
        var timeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(10, 0), new TimeOnly(11, 0));
        schedule.ScheduleAppointment("Jan Kowalski", timeSlot);
        
        var sameTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(10, 0), new TimeOnly(11, 0));

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment("Anna Nowak", sameTimeSlot));
        
        // Then
        exception!.Message.Should().Be("Appointment time slot conflicts with existing appointment");

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Should().HaveCount(1);
    }

    [Test]
    public void GivenScheduleWithAppointment_WhenScheduleAppointmentStartingDuringExisting_ThenThrowsArgumentException()
    {
        // Given
        var schedule = new DailyAppointmentSchedule(_scheduleDate);
        var firstTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(10, 0), new TimeOnly(11, 0));
        schedule.ScheduleAppointment("Jan Kowalski", firstTimeSlot);
        
        var overlappingTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(10, 45), new TimeOnly(12, 0));

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment("Anna Nowak", overlappingTimeSlot));
        
        // Then
        exception!.Message.Should().Be("Appointment time slot conflicts with existing appointment");

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Should().HaveCount(1);
    }

    [Test]
    public void GivenScheduleWithAppointment_WhenScheduleAppointmentEndingDuringExisting_ThenThrowsArgumentException()
    {
        // Given
        var schedule = new DailyAppointmentSchedule(_scheduleDate);
        var firstTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(10, 0), new TimeOnly(11, 0));
        schedule.ScheduleAppointment("Jan Kowalski", firstTimeSlot);
        
        var overlappingTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(9, 0), new TimeOnly(10, 30));

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment("Anna Nowak", overlappingTimeSlot));
        
        // Then
        exception!.Message.Should().Be("Appointment time slot conflicts with existing appointment");

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Should().HaveCount(1);
    }

    [Test]
    public void GivenScheduleWithAppointment_WhenScheduleAppointmentCompletelyEnclosingExisting_ThenThrowsArgumentException()
    {
        // Given
        var schedule = new DailyAppointmentSchedule(_scheduleDate);
        var firstTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(10, 0), new TimeOnly(11, 0));
        schedule.ScheduleAppointment("Jan Kowalski", firstTimeSlot);
        
        var enclosingTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(9, 0), new TimeOnly(12, 0));

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment("Anna Nowak", enclosingTimeSlot));
        
        // Then
        exception!.Message.Should().Be("Appointment time slot conflicts with existing appointment");

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Should().HaveCount(1);
    }

    [Test]
    public void GivenScheduleWithAppointment_WhenScheduleAdjacentAppointmentBefore_ThenBothAreScheduled()
    {
        // Given
        var schedule = new DailyAppointmentSchedule(_scheduleDate);
        var firstTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(10, 0), new TimeOnly(11, 0));
        var firstAppointment = schedule.ScheduleAppointment("Jan Kowalski", firstTimeSlot);
        
        var adjacentTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(9, 0), new TimeOnly(10, 0));

        // When
        var secondAppointment = schedule.ScheduleAppointment("Anna Nowak", adjacentTimeSlot);

        // Then
        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Should().HaveCount(2);
        
        var firstSnapshot = snapshot.Appointments.First(a => a.Id == firstAppointment.Id);
        firstSnapshot.StartTime.Should().Be(firstTimeSlot.StartDateTime);
        firstSnapshot.EndTime.Should().Be(firstTimeSlot.EndDateTime);
        
        var secondSnapshot = snapshot.Appointments.First(a => a.Id == secondAppointment.Id);
        secondSnapshot.StartTime.Should().Be(adjacentTimeSlot.StartDateTime);
        secondSnapshot.EndTime.Should().Be(adjacentTimeSlot.EndDateTime);
    }

    [Test]
    public void GivenScheduleWithAppointment_WhenScheduleAdjacentAppointmentAfter_ThenBothAreScheduled()
    {
        // Given
        var schedule = new DailyAppointmentSchedule(_scheduleDate);
        var firstTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(10, 0), new TimeOnly(11, 0));
        var firstAppointment = schedule.ScheduleAppointment("Jan Kowalski", firstTimeSlot);
        
        var adjacentTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(11, 0), new TimeOnly(12, 0));

        // When
        var secondAppointment = schedule.ScheduleAppointment("Anna Nowak", adjacentTimeSlot);

        // Then
        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Should().HaveCount(2);
        
        var firstSnapshot = snapshot.Appointments.First(a => a.Id == firstAppointment.Id);
        firstSnapshot.StartTime.Should().Be(firstTimeSlot.StartDateTime);
        firstSnapshot.EndTime.Should().Be(firstTimeSlot.EndDateTime);
        
        var secondSnapshot = snapshot.Appointments.First(a => a.Id == secondAppointment.Id);
        secondSnapshot.StartTime.Should().Be(adjacentTimeSlot.StartDateTime);
        secondSnapshot.EndTime.Should().Be(adjacentTimeSlot.EndDateTime);
    }

    [Test]
    public void GivenScheduleWithMultipleAppointments_WhenScheduleConflictingAppointment_ThenThrowsArgumentException()
    {
        // Given
        var schedule = new DailyAppointmentSchedule(_scheduleDate);
        
        var firstTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(9, 0), new TimeOnly(10, 0));
        schedule.ScheduleAppointment("Jan Kowalski", firstTimeSlot);
        
        var secondTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(11, 0), new TimeOnly(12, 0));
        schedule.ScheduleAppointment("Anna Nowak", secondTimeSlot);
        
        var conflictingTimeSlot = new AppointmentTimeSlot(_scheduleDate, new TimeOnly(8, 30), new TimeOnly(9, 30));

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment("Piotr Nowak", conflictingTimeSlot));
        
        // Then
        exception!.Message.Should().Be("Appointment time slot conflicts with existing appointment");

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Should().HaveCount(2);
    }
}