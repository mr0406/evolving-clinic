using EvolvingClinic.Application.Common;
using EvolvingClinic.Application.Doctors.Commands;
using EvolvingClinic.Application.Doctors.Queries;
using Reqnroll;
using Shouldly;

namespace EvolvingClinic.BusinessTests.StepDefinitions;

[Binding]
public sealed class RegisterDoctorStepDefinitions
{
    private readonly Dispatcher _dispatcher = new();
    private string? _scenarioDoctorCode;
    private Exception? _scenarioException;

    [When("I register a doctor:")]
    public async Task WhenIRegisterADoctor(Table table)
    {
        var row = table.Rows[0];
        var code = row["Code"];
        var firstName = row["First Name"];
        var lastName = row["Last Name"];

        try
        {
            _scenarioDoctorCode = await RegisterDoctor(code, firstName, lastName);
        }
        catch (Exception ex)
        {
            _scenarioException = ex;
            _scenarioDoctorCode = null;
        }
    }

    [Given("doctor {string} {string} is registered")]
    public async Task GivenDoctorIsRegistered(string firstName, string lastName)
    {
        var code = lastName.ToUpperInvariant();
        var command = new RegisterDoctorCommand(
            code,
            new RegisterDoctorCommand.PersonNameData(firstName, lastName));

        await _dispatcher.Execute(command);
    }

    [Then("the registered doctor should be:")]
    public async Task ThenTheRegisteredDoctorShouldBe(Table table)
    {
        _scenarioDoctorCode.ShouldNotBeNull();

        var query = new GetDoctorQuery(_scenarioDoctorCode);
        var doctor = await _dispatcher.ExecuteQuery(query);

        doctor.ShouldNotBeNull();

        var expectedRow = table.Rows[0];

        doctor.Code.ShouldBe(expectedRow["Code"]);
        doctor.Name.FirstName.ShouldBe(expectedRow["First Name"]);
        doctor.Name.LastName.ShouldBe(expectedRow["Last Name"]);
    }

    private async Task<string> RegisterDoctor(string code, string firstName, string lastName)
    {
        var command = new RegisterDoctorCommand(
            code,
            new RegisterDoctorCommand.PersonNameData(firstName, lastName));

        return await _dispatcher.Execute(command);
    }
}