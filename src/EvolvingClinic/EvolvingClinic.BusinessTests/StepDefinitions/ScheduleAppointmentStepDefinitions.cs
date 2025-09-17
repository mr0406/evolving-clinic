using EvolvingClinic.Application.Appointments.Commands;
using EvolvingClinic.Application.Appointments.Queries;
using EvolvingClinic.Application.Common;
using EvolvingClinic.Application.Doctors.Commands;
using EvolvingClinic.Application.Doctors.Queries;
using EvolvingClinic.Application.HealthcareServices.Queries;
using EvolvingClinic.Application.Patients.Queries;
using Reqnroll;
using Shouldly;

namespace EvolvingClinic.BusinessTests.StepDefinitions;

[Binding]
public sealed class ScheduleAppointmentStepDefinitions
{
    private readonly Dispatcher _dispatcher = new();
    private Guid? _scenarioAppointmentId;
    
    [Given("I registered doctors:")]
    public async Task GivenIRegisteredDoctors(Table table)
    {
        foreach (var row in table.Rows)
        {
            var code = row["Code"];
            var firstName = row["First Name"];
            var lastName = row["Last Name"];

            var command = new RegisterDoctorCommand(
                code,
                new RegisterDoctorCommand.PersonNameData(firstName, lastName));

            await _dispatcher.Execute(command);
        }
    }

    [Given("I scheduled appointment on {string}:")]
    public async Task GivenIScheduledAppointmentOn(string dateString, Table table)
    {
        var date = DateOnly.Parse(dateString);

        foreach (var row in table.Rows)
        {
            var patientName = row["Patient Name"];
            var doctorCode = row["Doctor Code"];
            var serviceCode = row["Healthcare Service Code"];
            var startTime = TimeOnly.Parse(row["Start Time"]);

            var nameParts = patientName.Split(' ');
            var firstName = nameParts[0];
            var lastName = nameParts[1];

            await ScheduleAppointment(date, doctorCode, firstName, lastName, serviceCode, startTime);
        }
    }

    [When("I schedule appointment on {string}:")]
    public async Task WhenIScheduleAppointmentOn(string dateString, Table table)
    {
        var date = DateOnly.Parse(dateString);

        var row = table.Rows[0]; // Only process the first row for the "When" step
        var patientName = row["Patient Name"];
        var doctorCode = row["Doctor Code"];
        var serviceCode = row["Healthcare Service Code"];
        var startTime = TimeOnly.Parse(row["Start Time"]);

        var nameParts = patientName.Split(' ');
        var firstName = nameParts[0];
        var lastName = nameParts[1];

        try
        {
            _scenarioAppointmentId = await ScheduleAppointment(date, doctorCode, firstName, lastName, serviceCode, startTime);
        }
        catch (Exception)
        {
            _scenarioAppointmentId = null;
        }
    }


    [Then("the appointment should be scheduled successfully")]
    public void ThenTheAppointmentShouldBeScheduledSuccessfully()
    {
        _scenarioAppointmentId.ShouldNotBeNull();
        _scenarioAppointmentId.ShouldNotBe(Guid.Empty);
    }

    [Then("there should be {int} appointment for doctor {string} on {string}")]
    [Then("there should be {int} appointments for doctor {string} on {string}")]
    public async Task ThenThereShouldBeAppointments(int expectedCount, string doctorCode, string dateString)
    {
        var date = DateOnly.Parse(dateString);
        var query = new GetDailyAppointmentScheduleQuery(doctorCode, date);
        var schedule = await _dispatcher.ExecuteQuery(query);

        schedule.ShouldNotBeNull();
        schedule.Date.ShouldBe(date);
        schedule.Appointments.Count.ShouldBe(expectedCount);
    }

    [Then("the scheduled appointment should be:")]
    public async Task ThenTheScheduledAppointmentShouldBe(Table table)
    {
        _scenarioAppointmentId.ShouldNotBeNull();

        var expectedRow = table.Rows[0];
        var doctorCode = expectedRow["Doctor Code"];
        var dateString = expectedRow["Date"];
        var date = DateOnly.Parse(dateString);

        var query = new GetDailyAppointmentScheduleQuery(doctorCode, date);
        var schedule = await _dispatcher.ExecuteQuery(query);

        schedule.ShouldNotBeNull();
        var appointment = schedule.Appointments.Single(a => a.Id == _scenarioAppointmentId.Value);

        // Get patient name
        var patientQuery = new GetPatientQuery(appointment.PatientId);
        var patient = await _dispatcher.ExecuteQuery(patientQuery);
        var patientName = $"{patient.Name.FirstName} {patient.Name.LastName}";

        // Get service type details
        var serviceTypeQuery = new GetHealthcareServiceTypeQuery(appointment.HealthcareServiceTypeCode);
        var serviceType = await _dispatcher.ExecuteQuery(serviceTypeQuery);

        patientName.ShouldBe(expectedRow["Patient Name"]);
        schedule.DoctorCode.ShouldBe(expectedRow["Doctor Code"]);
        appointment.HealthcareServiceTypeCode.ShouldBe(expectedRow["Healthcare Service Code"]);
        serviceType.Name.ShouldBe(expectedRow["Healthcare Service Name"]);
        appointment.StartTime.ToString("yyyy-MM-dd").ShouldBe(expectedRow["Date"]);
        appointment.StartTime.ToString("HH:mm").ShouldBe(expectedRow["Start Time"]);
        appointment.EndTime.ToString("HH:mm").ShouldBe(expectedRow["End Time"]);
        $"${appointment.Price}".ShouldBe(expectedRow["Price"]);
    }
    
    private async Task<Guid?> ScheduleAppointment(
        DateOnly date,
        string doctorCode,
        string firstName,
        string lastName,
        string serviceCode,
        TimeOnly startTime)
    {
        var getAllPatientsQuery = new GetAllPatientsQuery();
        var allPatients = await _dispatcher.ExecuteQuery(getAllPatientsQuery);

        var patient = allPatients.SingleOrDefault(p =>
            p.Name.FirstName == firstName && p.Name.LastName == lastName);

        if (patient == null)
        {
            throw new InvalidOperationException($"Patient {firstName} {lastName} is not registered. Please register the patient first.");
        }

        var command = new ScheduleAppointmentCommand(
            doctorCode,
            date,
            patient.Id,
            serviceCode,
            startTime);

        return await _dispatcher.Execute(command);
    }
}