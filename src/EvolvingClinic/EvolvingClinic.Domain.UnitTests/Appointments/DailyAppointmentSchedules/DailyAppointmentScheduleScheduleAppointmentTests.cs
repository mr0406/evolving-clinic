using EvolvingClinic.Domain.Appointments;
using FluentAssertions;
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
        var firstTimeSlot = new AppointmentTimeSlot(scheduleDate, new TimeOnly(9, 0), new TimeOnly(10, 0));
        var firstAppointment = schedule.ScheduleAppointment(firstPatient, firstTimeSlot);

        var secondPatient = "Anna Nowak";
        var secondTimeSlot = new AppointmentTimeSlot(scheduleDate, new TimeOnly(11, 0), new TimeOnly(12, 0));

        // When
        var secondAppointment = schedule.ScheduleAppointment(secondPatient, secondTimeSlot);

        // Then
        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Should().HaveCount(2);

        var firstAppointmentSnapshot = snapshot.Appointments.First(a => a.Id == firstAppointment.Id);
        firstAppointmentSnapshot.PatientName.Should().Be(firstPatient);
        firstAppointmentSnapshot.StartTime.Should().Be(firstTimeSlot.StartDateTime);
        firstAppointmentSnapshot.EndTime.Should().Be(firstTimeSlot.EndDateTime);

        var secondAppointmentSnapshot = snapshot.Appointments.First(a => a.Id == secondAppointment.Id);
        secondAppointmentSnapshot.PatientName.Should().Be(secondPatient);
        secondAppointmentSnapshot.StartTime.Should().Be(secondTimeSlot.StartDateTime);
        secondAppointmentSnapshot.EndTime.Should().Be(secondTimeSlot.EndDateTime);
    }
    
    [Test]
    public void GivenDailyAppointmentScheduleWithoutAppointments_WhenScheduleValidAppointment_ThenIsScheduled()
    {
        // Given
        var scheduleDate = new DateOnly(2024, 1, 15);
        var schedule = new DailyAppointmentSchedule(scheduleDate);
        var patientName = "Jan Kowalski";
        var timeSlot = new AppointmentTimeSlot(scheduleDate, new TimeOnly(10, 0), new TimeOnly(11, 0));

        // When
        var appointment = schedule.ScheduleAppointment(patientName, timeSlot);

        // Then
        appointment.Should().NotBeNull();
        appointment.Id.Should().NotBe(Guid.Empty);

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Should().HaveCount(1);
        
        var appointmentSnapshot = snapshot.Appointments.First();
        appointmentSnapshot.Id.Should().Be(appointment.Id);
        appointmentSnapshot.PatientName.Should().Be(patientName);
        appointmentSnapshot.StartTime.Should().Be(timeSlot.StartDateTime);
        appointmentSnapshot.EndTime.Should().Be(timeSlot.EndDateTime);
    }

    [Test]
    public void GivenDailyAppointmentScheduleWithoutAppointments_WhenScheduleAppointmentWithDifferentDateThanSchedule_ThenThrowsArgumentException()
    {
        // Given
        var scheduleDate = new DateOnly(2024, 1, 15);
        var schedule = new DailyAppointmentSchedule(scheduleDate);
        var patientName = "Jan Kowalski";
        var wrongDate = new DateOnly(2024, 1, 16);
        var timeSlot = new AppointmentTimeSlot(wrongDate, new TimeOnly(10, 0), new TimeOnly(11, 0));

        // When
        var exception = Assert.Throws<ArgumentException>(() => 
            schedule.ScheduleAppointment(patientName, timeSlot));
        
        // Then
        exception!.Message.Should().Be("Appointment must be scheduled for the same date as the schedule");

        var snapshot = schedule.CreateSnapshot();
        snapshot.Appointments.Should().BeEmpty();
    }
}