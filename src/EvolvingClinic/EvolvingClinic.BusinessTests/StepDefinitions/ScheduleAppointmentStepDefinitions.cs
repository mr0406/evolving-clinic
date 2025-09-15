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
    private ScheduleAppointmentData? _scenarioScheduleAppointmentData;
    private Guid? _scenarioAppointmentId;


    [Given("I have scheduled an appointment for {string} {string} with service {string} on {string} at {string}")]
    public async Task GivenIHaveScheduledAnAppointmentFor(string firstName, string lastName, string serviceCode, string dateString, string startTimeString)
    {
        var date = DateOnly.Parse(dateString);
        var startTime = TimeOnly.Parse(startTimeString);

        await ScheduleAppointment(date, firstName, lastName, serviceCode, startTime);
    }
    
    
    [When("I schedule an appointment for {string} {string} with service {string} on {string} at {string}")]
    public async Task WhenIScheduleAnAppointmentFor(string firstName, string lastName, string serviceCode, string dateString, string startTimeString)
    {
        var date = DateOnly.Parse(dateString);
        var startTime = TimeOnly.Parse(startTimeString);

        _scenarioScheduleAppointmentData = new(date, serviceCode, startTime);

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

    [Then("there should be {int} appointments in the schedule")]
    public async Task ThenThereShouldBeAppointmentsInTheSchedule(int expectedCount)
    {
        _scenarioScheduleAppointmentData.ShouldNotBeNull();
        
        var query = new GetDailyAppointmentScheduleQuery(_scenarioScheduleAppointmentData.Date);
        var schedule = await _dispatcher.ExecuteQuery(query);

        schedule.ShouldNotBeNull();
        schedule.Date.ShouldBe(_scenarioScheduleAppointmentData.Date);
        schedule.Appointments.Count.ShouldBe(expectedCount);

        if (_scenarioAppointmentId.HasValue)
        {
            var scenarioAppointment = schedule.Appointments.Single(a => a.Id == _scenarioAppointmentId.Value);
            scenarioAppointment.PatientId.ShouldNotBe(Guid.Empty);
            scenarioAppointment.HealthcareServiceTypeCode.ShouldBe(_scenarioScheduleAppointmentData.ServiceCode);
            scenarioAppointment.StartTime.ShouldBe(_scenarioScheduleAppointmentData.Date.ToDateTime(_scenarioScheduleAppointmentData.StartTime));

            var serviceTypeQuery = new GetHealthcareServiceTypeQuery(_scenarioScheduleAppointmentData.ServiceCode);
            var serviceType = await _dispatcher.ExecuteQuery(serviceTypeQuery);
            scenarioAppointment.Price.ShouldBe(serviceType.Price);
        }
    }

    [Then("the appointment should fail to be scheduled")]
    public void ThenTheAppointmentShouldFailToBeScheduled()
    {
        _scenarioAppointmentId.ShouldBeNull();
    }

    [Then("there should be no appointments in the schedule")]
    public async Task ThenThereShouldBeNoAppointmentsInTheSchedule()
    {
        await ThenThereShouldBeAppointmentsInTheSchedule(0);
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


    private record ScheduleAppointmentData(
        DateOnly Date,
        string ServiceCode,
        TimeOnly StartTime);

}