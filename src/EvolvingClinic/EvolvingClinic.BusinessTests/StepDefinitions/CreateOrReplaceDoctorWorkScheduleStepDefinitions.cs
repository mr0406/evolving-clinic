using EvolvingClinic.Application.Common;
using EvolvingClinic.Application.DoctorWorkSchedules.Commands;
using EvolvingClinic.Application.DoctorWorkSchedules.Queries;
using EvolvingClinic.Domain.Shared;
using Reqnroll;
using Shouldly;

namespace EvolvingClinic.BusinessTests.StepDefinitions;

[Binding]
public sealed class CreateOrReplaceDoctorWorkScheduleStepDefinitions
{
    private readonly Dispatcher _dispatcher = new();
    private Exception? _scenarioException;

    [Given("I created doctor work schedule for {string}:")]
    public async Task GivenICreatedDoctorWorkScheduleFor(string doctorCode, Table table)
    {
        await CreateOrReplaceDoctorWorkSchedule(doctorCode, table);
    }

    [When("I create or replace doctor work schedule for {string}:")]
    public async Task WhenICreateOrReplaceDoctorWorkScheduleFor(string doctorCode, Table table)
    {
        try
        {
            await CreateOrReplaceDoctorWorkSchedule(doctorCode, table);
        }
        catch (Exception ex)
        {
            _scenarioException = ex;
        }
    }

    [Then("the doctor work schedule for {string} should be:")]
    public async Task ThenTheDoctorWorkScheduleForShouldBe(string doctorCode, Table table)
    {
        var query = new GetDoctorWorkScheduleQuery(doctorCode);
        var schedule = await _dispatcher.ExecuteQuery(query);

        schedule.ShouldNotBeNull();
        schedule.DoctorCode.ShouldBe(doctorCode);

        schedule.WeeklySchedule.Count.ShouldBe(table.RowCount);

        foreach (var expectedRow in table.Rows)
        {
            var dayOfWeek = Enum.Parse<DayOfWeek>(expectedRow["Day"]);
            var expectedStartTime = TimeOnly.Parse(expectedRow["Start Time"]);
            var expectedEndTime = TimeOnly.Parse(expectedRow["End Time"]);
            var expectedTimeRange = new TimeRange(expectedStartTime, expectedEndTime);

            schedule.WeeklySchedule.ShouldContainKey(dayOfWeek);
            schedule.WeeklySchedule[dayOfWeek].ShouldBe(expectedTimeRange);
        }
    }

    [Then("I should get an error {string}")]
    public void ThenIShouldGetAnError(string expectedErrorMessage)
    {
        _scenarioException.ShouldNotBeNull();
        _scenarioException.Message.ShouldBe(expectedErrorMessage);
    }

    private async Task CreateOrReplaceDoctorWorkSchedule(string doctorCode, Table table)
    {
        var workingDays = table.Rows.Select(row =>
        {
            var dayOfWeek = Enum.Parse<DayOfWeek>(row["Day"]);
            var startTime = TimeOnly.Parse(row["Start Time"]);
            var endTime = TimeOnly.Parse(row["End Time"]);
            var timeRange = new TimeRange(startTime, endTime);

            return new CreateOrReplaceDoctorWorkScheduleCommand.WorkingDayData(dayOfWeek, timeRange);
        }).ToList();

        var command = new CreateOrReplaceDoctorWorkScheduleCommand(doctorCode, workingDays);
        await _dispatcher.Execute(command);
    }
}