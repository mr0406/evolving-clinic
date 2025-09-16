using EvolvingClinic.Application.Appointments.Commands;
using EvolvingClinic.Application.Appointments.Queries;
using EvolvingClinic.Application.Common;
using EvolvingClinic.Application.HealthcareServices.Queries;
using EvolvingClinic.Application.Patients.Queries;
using Reqnroll;
using Shouldly;

namespace EvolvingClinic.BusinessTests.StepDefinitions;

[Binding]
public sealed class ScheduleAppointmentStepDefinitions
{
    private readonly Dispatcher _dispatcher = new();
    private DateOnly _scenarioDate;
    private Guid? _scenarioAppointmentId;


    [Given("I have scheduled an appointment for {string} {string} with healthcare service type {string} on {string} at {string}")]
    public async Task GivenIHaveScheduledAnAppointmentFor(string firstName, string lastName, string serviceCode, string dateString, string startTimeString)
    {
        var date = DateOnly.Parse(dateString);
        var startTime = TimeOnly.Parse(startTimeString);

        _scenarioDate = date;

        await ScheduleAppointment(date, firstName, lastName, serviceCode, startTime);
    }
    
    
    [When("I schedule an appointment for {string} {string} with healthcare service type {string} on {string} at {string}")]
    public async Task WhenIScheduleAnAppointmentFor(string firstName, string lastName, string serviceCode, string dateString, string startTimeString)
    {
        var date = DateOnly.Parse(dateString);
        var startTime = TimeOnly.Parse(startTimeString);

        _scenarioDate = date;

        try
        {
            _scenarioAppointmentId = await ScheduleAppointment(date, firstName, lastName, serviceCode, startTime);
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

    [Then("there should be {int} appointment")]
    [Then("there should be {int} appointments")]
    public async Task ThenThereShouldBeAppointments(int expectedCount)
    {
        var query = new GetDailyAppointmentScheduleQuery(_scenarioDate);
        var schedule = await _dispatcher.ExecuteQuery(query);

        schedule.ShouldNotBeNull();
        schedule.Date.ShouldBe(_scenarioDate);
        schedule.Appointments.Count.ShouldBe(expectedCount);
    }

    [Then("the scheduled appointment should be:")]
    public async Task ThenTheScheduledAppointmentShouldBe(Table table)
    {
        _scenarioAppointmentId.ShouldNotBeNull();

        var query = new GetDailyAppointmentScheduleQuery(_scenarioDate);
        var schedule = await _dispatcher.ExecuteQuery(query);

        schedule.ShouldNotBeNull();
        var appointment = schedule.Appointments.Single(a => a.Id == _scenarioAppointmentId.Value);

        var expectedRow = table.Rows[0];

        // Get patient name
        var patientQuery = new GetPatientQuery(appointment.PatientId);
        var patient = await _dispatcher.ExecuteQuery(patientQuery);
        var patientName = $"{patient.Name.FirstName} {patient.Name.LastName}";

        // Get service type details
        var serviceTypeQuery = new GetHealthcareServiceTypeQuery(appointment.HealthcareServiceTypeCode);
        var serviceType = await _dispatcher.ExecuteQuery(serviceTypeQuery);

        patientName.ShouldBe(expectedRow["Patient Name"]);
        appointment.HealthcareServiceTypeCode.ShouldBe(expectedRow["Healthcare Service Code"]);
        serviceType.Name.ShouldBe(expectedRow["Healthcare Service Name"]);
        appointment.StartTime.ToString("yyyy-MM-dd").ShouldBe(expectedRow["Date"]);
        appointment.StartTime.ToString("HH:mm").ShouldBe(expectedRow["Start Time"]);
        appointment.EndTime.ToString("HH:mm").ShouldBe(expectedRow["End Time"]);
        $"${appointment.Price}".ShouldBe(expectedRow["Price"]);
    }

    
    private async Task<Guid?> ScheduleAppointment(
        DateOnly date,
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
            date,
            patient.Id,
            serviceCode,
            startTime);

        return await _dispatcher.Execute(command);
    }



}