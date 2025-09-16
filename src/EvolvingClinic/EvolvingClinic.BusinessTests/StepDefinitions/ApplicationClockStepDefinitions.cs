using EvolvingClinic.Domain.Utils;
using Reqnroll;

namespace EvolvingClinic.BusinessTests.StepDefinitions;

[Binding]
public class ApplicationClockStepDefinitions
{
    [Given(@"'(.*)' date")]
    [Given(@"""(.*)"" date")]
    public void GivenDate(string date)
    {
        ApplicationClock.SetDate(DateOnly.Parse(date));
    }

    [When(@"'(.*)' date")]
    [When(@"""(.*)"" date")]
    public void WhenDate(string date)
    {
        ApplicationClock.SetDate(DateOnly.Parse(date));
    }
}