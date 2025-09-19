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
        var firstAppointment = schedule.ScheduleAppointment(firstPatientId, "TEST", new TimeOnly(9, 0), new TimeOnly(10, 0), new Money(100.00m));

        var secondPatientId = Guid.NewGuid();

        // When
        var secondAppointment = schedule.ScheduleAppointment(secondPatientId, "TEST", new TimeOnly(11, 0), new TimeOnly(12, 0), new Money(100.00m));

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
        var appointment = schedule.ScheduleAppointment(patientId, "TEST", new TimeOnly(10, 0), new TimeOnly(11, 0), new Money(100.00m));

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
        var appointment = schedule.ScheduleAppointment(patientId, "FREE", new TimeOnly(10, 0), new TimeOnly(11, 0), price);

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
        var exception = Should.Throw<ArgumentException>(() =>
            schedule.ScheduleAppointment(patientId, "TEST", new TimeOnly(10, 0), new TimeOnly(11, 0), price));

        // Then
        exception!.Message.ShouldBe("Appointment price must be 0 or greater");
    }
}