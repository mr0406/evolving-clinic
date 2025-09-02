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

        var firstPatient = "Jan Kowalski";
        var firstAppointment = schedule.ScheduleAppointment(firstPatient, new TimeOnly(9, 0), new TimeOnly(10, 0));

        var secondPatient = "Anna Nowak";

        // When
        var secondAppointment = schedule.ScheduleAppointment(secondPatient, new TimeOnly(11, 0), new TimeOnly(12, 0));

        // Then
        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Count.ShouldBe(2);

        var firstAppointmentSnapshot = snapshot.Appointments.First(a => a.Id == firstAppointment.Id);
        firstAppointmentSnapshot.PatientName.ShouldBe(firstPatient);
        firstAppointmentSnapshot.StartTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(9, 0)));
        firstAppointmentSnapshot.EndTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(10, 0)));

        var secondAppointmentSnapshot = snapshot.Appointments.First(a => a.Id == secondAppointment.Id);
        secondAppointmentSnapshot.PatientName.ShouldBe(secondPatient);
        secondAppointmentSnapshot.StartTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(11, 0)));
        secondAppointmentSnapshot.EndTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(12, 0)));
    }
    
    [Test]
    public void GivenDailyAppointmentScheduleWithoutAppointments_WhenScheduleValidAppointment_ThenIsScheduled()
    {
        // Given
        var scheduleDate = new DateOnly(2024, 1, 15);
        var schedule = new DailyAppointmentSchedule(scheduleDate);
        var patientName = "Jan Kowalski";

        // When
        var appointment = schedule.ScheduleAppointment(patientName, new TimeOnly(10, 0), new TimeOnly(11, 0));

        // Then
        appointment.ShouldNotBeNull();
        appointment.Id.ShouldNotBe(Guid.Empty);

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Count.ShouldBe(1);
        
        var appointmentSnapshot = snapshot.Appointments.First();
        appointmentSnapshot.Id.ShouldBe(appointment.Id);
        appointmentSnapshot.PatientName.ShouldBe(patientName);
        appointmentSnapshot.StartTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(10, 0)));
        appointmentSnapshot.EndTime.ShouldBe(scheduleDate.ToDateTime(new TimeOnly(11, 0)));
    }
}