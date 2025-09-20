using EvolvingClinic.Application.DoctorWorkSchedules.Commands;
using EvolvingClinic.Application.DoctorWorkSchedules.Queries;
using EvolvingClinic.BusinessTests.Utils;
using EvolvingClinic.Domain.Shared;
using Reqnroll;
using Shouldly;

namespace EvolvingClinic.BusinessTests.StepDefinitions;

[Binding]
public sealed class CreateOrReplaceDoctorWorkScheduleStepDefinitions
{
    [Given("I created doctor work schedule for {string}:")]
    public async Task GivenICreatedDoctorWorkScheduleFor(string doctorCode, Table table)
    {
        await CreateOrReplaceDoctorWorkSchedule(doctorCode, table);
    }

    [When("I create or replace doctor work schedule for {string}:")]
    public async Task WhenICreateOrReplaceDoctorWorkScheduleFor(string doctorCode, Table table)
    {
        await CreateOrReplaceDoctorWorkSchedule(doctorCode, table);
    }

    [Then("the doctor work schedule for {string} should be:")]
    public async Task ThenTheDoctorWorkScheduleForShouldBe(string doctorCode, Table table)
    {
        var query = new GetDoctorWorkScheduleQuery(doctorCode);
        var schedule = await TestDispatcher.ExecuteQuery(query);

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
        var lastError = TestErrorContext.GetLastError();
        lastError.ShouldNotBeNull();
        lastError.Message.ShouldBe(expectedErrorMessage);
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
        await TestDispatcher.Execute(command);
    }
}