using EvolvingClinic.Application.Appointments.Commands;
using EvolvingClinic.Application.Appointments.Queries;
using EvolvingClinic.Application.Common;
using EvolvingClinic.Application.Patients.Commands;
using Reqnroll;
using Shouldly;

namespace EvolvingClinic.BusinessTests.StepDefinitions;

[Binding]
public sealed class ScheduleAppointmentStepDefinitions
{
    private readonly Dispatcher _dispatcher = new();
    private ScheduleAppointmentData? _scenarioScheduleAppointmentData;
    private Guid? _scenarioAppointmentId;
    private readonly Dictionary<string, Guid> _registeredPatients = new();
    
    [Given("patient {string} {string} is registered")]
    public async Task GivenPatientIsRegistered(string firstName, string lastName)
    {
        var patientKey = $"{firstName} {lastName}";
        
        var registerCommand = new RegisterPatientCommand(
            new RegisterPatientCommand.PersonNameData(firstName, lastName),
            new DateOnly(1990, 1, 1),
            new RegisterPatientCommand.PhoneNumberData("+1", "5551234567"),
            new RegisterPatientCommand.AddressData("Test Street", "123", null, "12345", "Test City"));

        var patientId = await _dispatcher.Execute(registerCommand);
        _registeredPatients[patientKey] = patientId;
    }

    [Given("I have scheduled an appointment for {string} {string} on {string} from {string} to {string}")]
    public async Task GivenIHaveScheduledAnAppointmentFor(string firstName, string lastName, string dateString, string startTimeString, string endTimeString)
    {
        var date = DateOnly.Parse(dateString);
        var startTime = TimeOnly.Parse(startTimeString);
        var endTime = TimeOnly.Parse(endTimeString);
        
        await ScheduleAppointment(date, firstName, lastName, startTime, endTime);
    }
    
    [Given("I have scheduled an appointment for {string} on {string} from {string} to {string}")]
    public async Task GivenIHaveScheduledAnAppointmentFor(string patientName, string dateString, string startTimeString, string endTimeString)
    {
        var date = DateOnly.Parse(dateString);
        var startTime = TimeOnly.Parse(startTimeString);
        var endTime = TimeOnly.Parse(endTimeString);
        
        await ScheduleAppointment(date, patientName, startTime, endTime);
    }
    
    [When("I schedule an appointment for {string} {string} on {string} from {string} to {string}")]
    public async Task WhenIScheduleAnAppointmentFor(string firstName, string lastName, string dateString, string startTimeString, string endTimeString)
    {
        var date = DateOnly.Parse(dateString);
        var startTime = TimeOnly.Parse(startTimeString);
        var endTime = TimeOnly.Parse(endTimeString);
        
        _scenarioScheduleAppointmentData = new(date, startTime, endTime);
        
        try
        {
            _scenarioAppointmentId = await ScheduleAppointment(date, firstName, lastName, startTime, endTime);
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
            scenarioAppointment.StartTime.ShouldBe(_scenarioScheduleAppointmentData.Date.ToDateTime(_scenarioScheduleAppointmentData.StartTime));
            scenarioAppointment.EndTime.ShouldBe(_scenarioScheduleAppointmentData.Date.ToDateTime(_scenarioScheduleAppointmentData.EndTime));
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
        TimeOnly startTime,
        TimeOnly endTime)
    {
        var patientKey = $"{firstName} {lastName}";
        
        if (!_registeredPatients.TryGetValue(patientKey, out var patientId))
        {
            throw new InvalidOperationException($"Patient {patientKey} is not registered. Please register the patient first.");
        }
        
        var command = new ScheduleAppointmentCommand(
            date,
            patientId,
            startTime,
            endTime);
        
        return await _dispatcher.Execute(command);
    }

    private async Task ScheduleAppointment(
        DateOnly date,
        string patientName,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        var nameParts = patientName.Split(' ');
        var firstName = nameParts[0];
        var lastName = nameParts.Length > 1 ? nameParts[1] : "";
        
        await ScheduleAppointment(date, firstName, lastName, startTime, endTime);
    }

    private record ScheduleAppointmentData(
        DateOnly Date,
        TimeOnly StartTime,
        TimeOnly EndTime);
}