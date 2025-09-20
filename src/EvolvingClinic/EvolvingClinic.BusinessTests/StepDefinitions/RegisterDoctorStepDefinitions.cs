using EvolvingClinic.Application.Doctors.Commands;
using EvolvingClinic.Application.Doctors.Queries;
using EvolvingClinic.BusinessTests.Utils;
using Reqnroll;
using Shouldly;

namespace EvolvingClinic.BusinessTests.StepDefinitions;

[Binding]
public sealed class RegisterDoctorStepDefinitions
{
    private string? _scenarioDoctorCode;

    [When("I register a doctor:")]
    public async Task WhenIRegisterADoctor(Table table)
    {
        var row = table.Rows[0];
        var code = row["Code"];
        var firstName = row["First Name"];
        var lastName = row["Last Name"];

        _scenarioDoctorCode = await RegisterDoctor(code, firstName, lastName);
    }

    [Given("doctor {string} {string} is registered")]
    public async Task GivenDoctorIsRegistered(string firstName, string lastName)
    {
        var code = lastName.ToUpperInvariant();
        var command = new RegisterDoctorCommand(
            code,
            new RegisterDoctorCommand.PersonNameData(firstName, lastName));

        await TestDispatcher.Execute(command);
    }

    [Then("the registered doctor should be:")]
    public async Task ThenTheRegisteredDoctorShouldBe(Table table)
    {
        _scenarioDoctorCode.ShouldNotBeNull();

        var query = new GetDoctorQuery(_scenarioDoctorCode);
        var doctor = await TestDispatcher.ExecuteQuery(query);

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

        return await TestDispatcher.Execute(command);
    }
}