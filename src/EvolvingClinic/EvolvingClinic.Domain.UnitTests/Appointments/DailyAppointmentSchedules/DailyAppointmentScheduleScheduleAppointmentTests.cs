using EvolvingClinic.Domain.Appointments;
using Shouldly;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests.Appointments.DailyAppointmentSchedules;

[TestFixture]
public class DailyAppointmentScheduleScheduleAppointmentTests
{
    [Test]
    public void GivenDailyAppointmentScheduleWithOneAppointment_WhenScheduleSecondValidAppointment_ThenBothAreScheduled()
    {
        // Given
        var scheduleDate = new DateOnly(2024, 1, 15);
        var schedule = new DailyAppointmentSchedule(scheduleDate);

        var firstPatientId = Guid.NewGuid();
        var firstAppointment = schedule.ScheduleAppointment(firstPatientId, new TimeOnly(9, 0), new TimeOnly(10, 0));

        var secondPatientId = Guid.NewGuid();

        // When
        var secondAppointment = schedule.ScheduleAppointment(secondPatientId, new TimeOnly(11, 0), new TimeOnly(12, 0));

        // Then
        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Count.ShouldBe(2);

        var firstAppointmentSnapshot = snapshot.Appointments.First(a => a.Id == firstAppointment.Id);
        firstAppointmentSnapshot.PatientId.ShouldBe(firstPatientId);
        firstAppointmentSnapshot.StartTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(9, 0)));
        firstAppointmentSnapshot.EndTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(10, 0)));

        var secondAppointmentSnapshot = snapshot.Appointments.First(a => a.Id == secondAppointment.Id);
        secondAppointmentSnapshot.PatientId.ShouldBe(secondPatientId);
        secondAppointmentSnapshot.StartTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(11, 0)));
        secondAppointmentSnapshot.EndTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(12, 0)));
    }
    
    [Test]
    public void GivenDailyAppointmentScheduleWithoutAppointments_WhenScheduleValidAppointment_ThenIsScheduled()
    {
        // Given
        var scheduleDate = new DateOnly(2024, 1, 15);
        var schedule = new DailyAppointmentSchedule(scheduleDate);
        var patientId = Guid.NewGuid();

        // When
        var appointment = schedule.ScheduleAppointment(patientId, new TimeOnly(10, 0), new TimeOnly(11, 0));

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
    }
}