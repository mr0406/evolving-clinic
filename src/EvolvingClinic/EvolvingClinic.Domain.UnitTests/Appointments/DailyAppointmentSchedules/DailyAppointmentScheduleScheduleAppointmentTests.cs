using EvolvingClinic.Domain.Appointments;
using EvolvingClinic.Domain.Shared;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Appointments.DailyAppointmentSchedules;

public class DailyAppointmentScheduleScheduleAppointmentTests : TestBase
{
    [Test]
    public void GivenDailyAppointmentScheduleWithOneAppointment_WhenScheduleSecondValidAppointment_ThenBothAreScheduled()
    {
        // Given
        var scheduleDate = new DateOnly(2024, 1, 15);
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = DailyAppointmentSchedule.Create(new DailyAppointmentSchedule.Key("SMITH", scheduleDate), workingHours);

        var firstPatientId = Guid.NewGuid();
        var firstAppointment = schedule.ScheduleAppointment(firstPatientId, "TEST", new TimeRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), new Money(100.00m));

        var secondPatientId = Guid.NewGuid();

        // When
        var secondAppointment = schedule.ScheduleAppointment(secondPatientId, "TEST", new TimeRange(new TimeOnly(11, 0), new TimeOnly(12, 0)), new Money(100.00m));

        // Then
        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Count.ShouldBe(2);

        var firstAppointmentSnapshot = snapshot.Appointments.First(a => a.Id == firstAppointment.Id);
        firstAppointmentSnapshot.PatientId.ShouldBe(firstPatientId);
        firstAppointmentSnapshot.StartTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(9, 0)));
        firstAppointmentSnapshot.EndTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(10, 0)));
        firstAppointmentSnapshot.Price.ShouldBe(new Money(100.00m));

        var secondAppointmentSnapshot = snapshot.Appointments.First(a => a.Id == secondAppointment.Id);
        secondAppointmentSnapshot.PatientId.ShouldBe(secondPatientId);
        secondAppointmentSnapshot.StartTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(11, 0)));
        secondAppointmentSnapshot.EndTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(12, 0)));
        secondAppointmentSnapshot.Price.ShouldBe(new Money(100.00m));
    }
    
    [Test]
    public void GivenDailyAppointmentScheduleWithoutAppointments_WhenScheduleValidAppointment_ThenIsScheduled()
    {
        // Given
        var scheduleDate = new DateOnly(2024, 1, 15);
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = DailyAppointmentSchedule.Create(new DailyAppointmentSchedule.Key("SMITH", scheduleDate), workingHours);
        var patientId = Guid.NewGuid();

        // When
        var appointment = schedule.ScheduleAppointment(patientId, "TEST", new TimeRange(new TimeOnly(10, 0), new TimeOnly(11, 0)), new Money(100.00m));

        // Then
        appointment.ShouldNotBeNull();
        appointment.Id.ShouldNotBe(Guid.Empty);

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Count.ShouldBe(1);
        
        var appointmentSnapshot = snapshot.Appointments.First();
        appointmentSnapshot.Id.ShouldBe(appointment.Id);
        appointmentSnapshot.PatientId.ShouldBe(patientId);
        appointmentSnapshot.StartTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(10, 0)));
        appointmentSnapshot.EndTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(11, 0)));
        appointmentSnapshot.Price.ShouldBe(new Money(100.00m));
    }

    [Test]
    public void GivenZeroPrice_WhenScheduleAppointment_ThenIsScheduledWithZeroPrice()
    {
        // Given
        var scheduleDate = new DateOnly(2024, 1, 15);
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = DailyAppointmentSchedule.Create(new DailyAppointmentSchedule.Key("SMITH", scheduleDate), workingHours);
        var patientId = Guid.NewGuid();
        var price = new Money(0.00m);

        // When
        var appointment = schedule.ScheduleAppointment(patientId, "FREE", new TimeRange(new TimeOnly(10, 0), new TimeOnly(11, 0)), price);

        // Then
        appointment.ShouldNotBeNull();
        appointment.CreateSnapshot().Price.ShouldBe(price);
    }

    [Test]
    public void GivenNegativePrice_WhenScheduleAppointment_ThenThrowsArgumentException()
    {
        // Given
        var scheduleDate = new DateOnly(2024, 1, 15);
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = DailyAppointmentSchedule.Create(new DailyAppointmentSchedule.Key("SMITH", scheduleDate), workingHours);
        var patientId = Guid.NewGuid();
        var price = new Money(-10.00m);

        // When
        Action scheduleAppointment = () =>
            schedule.ScheduleAppointment(patientId, "TEST", new TimeRange(new TimeOnly(10, 0), new TimeOnly(11, 0)), price);

        // Then
        var exception = Should.Throw<ArgumentException>(scheduleAppointment);
        exception!.Message.ShouldBe("Appointment price must be 0 or greater");
    }

    [Test]
    public void GivenScheduleWithAppointment_WhenScheduleOverlappingAppointment_ThenThrowsArgumentException()
    {
        // Given
        var scheduleDate = new DateOnly(2024, 1, 15);
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = DailyAppointmentSchedule.Create(new DailyAppointmentSchedule.Key("SMITH", scheduleDate), workingHours);
        schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeRange(new TimeOnly(10, 0), new TimeOnly(11, 0)), new Money(100.00m));

        // When
        Action scheduleAppointment = () =>
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeRange(new TimeOnly(10, 30), new TimeOnly(11, 30)), new Money(100.00m));

        // Then
        var exception = Should.Throw<ArgumentException>(scheduleAppointment);
        exception.Message.ShouldBe("Appointment time slot conflicts with existing appointment");

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Count.ShouldBe(1);
    }

    [Test]
    public void GivenWorkingHours9To17_WhenScheduleAppointmentStartingBefore9_ThenThrowsArgumentException()
    {
        // Given
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = DailyAppointmentSchedule.Create(new DailyAppointmentSchedule.Key("SMITH", new DateOnly(2024, 1, 15)), workingHours);

        // When
        Action scheduleAppointment = () =>
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeRange(new TimeOnly(8, 30), new TimeOnly(9, 30)), new Money(100.00m));

        // Then
        var exception = Should.Throw<ArgumentException>(scheduleAppointment);
        exception.Message.ShouldBe("Appointments can only be scheduled between 09:00 and 17:00");
    }

    [Test]
    public void GivenWorkingHours9To17_WhenScheduleAppointmentEndingAfter17_ThenThrowsArgumentException()
    {
        // Given
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = DailyAppointmentSchedule.Create(new DailyAppointmentSchedule.Key("SMITH", new DateOnly(2024, 1, 15)), workingHours);

        // When
        Action scheduleAppointment = () =>
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeRange(new TimeOnly(16, 30), new TimeOnly(17, 30)), new Money(100.00m));

        // Then
        var exception = Should.Throw<ArgumentException>(scheduleAppointment);
        exception.Message.ShouldBe("Appointments can only be scheduled between 09:00 and 17:00");
    }

    [Test]
    public void GivenWorkingHours9To17_WhenScheduleAppointmentCompletelyOutside_ThenThrowsArgumentException()
    {
        // Given
        var workingHours = new TimeRange(new TimeOnly(9, 0), new TimeOnly(17, 0));
        var schedule = DailyAppointmentSchedule.Create(new DailyAppointmentSchedule.Key("SMITH", new DateOnly(2024, 1, 15)), workingHours);

        // When
        Action scheduleAppointment = () =>
            schedule.ScheduleAppointment(Guid.NewGuid(), "TEST", new TimeRange(new TimeOnly(18, 0), new TimeOnly(19, 0)), new Money(100.00m));

        // Then
        var exception = Should.Throw<ArgumentException>(scheduleAppointment);
        exception.Message.ShouldBe("Appointments can only be scheduled between 09:00 and 17:00");
    }
}