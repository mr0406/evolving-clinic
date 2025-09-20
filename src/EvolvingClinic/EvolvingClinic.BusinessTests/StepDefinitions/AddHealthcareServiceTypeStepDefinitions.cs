using EvolvingClinic.Application.HealthcareServices.Commands;
using EvolvingClinic.Application.HealthcareServices.Queries;
using EvolvingClinic.BusinessTests.Utils;
using EvolvingClinic.Domain.Utils;
using Reqnroll;
using Shouldly;

namespace EvolvingClinic.BusinessTests.StepDefinitions;

[Binding]
public sealed class AddHealthcareServiceTypeStepDefinitions
{
    private string? _scenarioServiceTypeCode;

    [Given("I added healthcare service types:")]
    public async Task GivenIAddedHealthcareServiceTypes(Table table)
    {
        foreach (var row in table.Rows)
        {
            var name = row["Healthcare Service Name"];
            var code = row["Code"];
            var duration = row["Duration"];
            var price = row["Price"];

            var timeSpan = ParseDuration(duration);
            var priceValue = ParsePrice(price);
            await AddHealthcareServiceType(name, code, timeSpan, priceValue);
        }
    }

    [Given("I added healthcare service type on {string}:")]
    public async Task GivenIAddedHealthcareServiceTypeOn(string dateString, Table table)
    {
        var date = DateOnly.Parse(dateString);
        ApplicationClock.SetDate(date);

        var row = table.Rows[0];
        var name = row["Healthcare Service Name"];
        var code = row["Code"];
        var duration = row["Duration"];
        var price = row["Price"];

        var timeSpan = ParseDuration(duration);
        var priceValue = ParsePrice(price);
        await AddHealthcareServiceType(name, code, timeSpan, priceValue);
        _scenarioServiceTypeCode = code;
    }

    [When("I add healthcare service type on {string}:")]
    public async Task WhenIAddHealthcareServiceTypeOn(string dateString, Table table)
    {
        var date = DateOnly.Parse(dateString);
        ApplicationClock.SetDate(date);

        var row = table.Rows[0];
        var name = row["Healthcare Service Name"];
        var code = row["Code"];
        var duration = row["Duration"];
        var price = row["Price"];

        var timeSpan = ParseDuration(duration);
        var priceValue = ParsePrice(price);

        await AddHealthcareServiceType(name, code, timeSpan, priceValue);
        _scenarioServiceTypeCode = code;
    }



    [Then("the added healthcare service type should be:")]
    public async Task ThenTheAddedHealthcareServiceTypeShouldBe(Table table)
    {
        _scenarioServiceTypeCode.ShouldNotBeNull();

        var query = new GetHealthcareServiceTypeQuery(_scenarioServiceTypeCode);
        var serviceType = await TestDispatcher.ExecuteQuery(query);

        serviceType.ShouldNotBeNull();

        var expectedRow = table.Rows[0];

        serviceType.Name.ShouldBe(expectedRow["Healthcare Service Name"]);
        serviceType.Code.ShouldBe(expectedRow["Code"]);
        serviceType.Duration.ShouldBe(ParseDuration(expectedRow["Duration"]));
        serviceType.Price.ShouldBe(ParsePrice(expectedRow["Current Price"]));
        
        serviceType.PriceHistory.Count.ShouldBe(1);
        var historyEntry = serviceType.PriceHistory[0];
        historyEntry.Price.ShouldBe(ParsePrice(expectedRow["Current Price"]));
        historyEntry.EffectiveFrom.ShouldBe(DateOnly.Parse(expectedRow["Price History From"]));
        historyEntry.EffectiveTo.ShouldBe(string.IsNullOrEmpty(expectedRow["Price History To"]) ? null : DateOnly.Parse(expectedRow["Price History To"]));
    }

    [Then("there should be {int} healthcare service type")]
    [Then("there should be {int} healthcare service types")]
    public async Task ThenThereShouldBeHealthcareServiceTypes(int expectedCount)
    {
        var query = new GetAllHealthcareServiceTypesQuery();
        var serviceTypes = await TestDispatcher.ExecuteQuery(query);
        serviceTypes.Count.ShouldBe(expectedCount);
    }

    private async Task AddHealthcareServiceType(string name, string code, TimeSpan duration, decimal price)
    {
        var command = new AddHealthcareServiceTypeCommand(name, code, duration, price);
        await TestDispatcher.Execute(command);
    }

    private static TimeSpan ParseDuration(string duration)
    {
        var parts = duration.Split(' ');
        var value = int.Parse(parts[0]);
        var unit = parts[1].ToLowerInvariant();

        return unit switch
        {
            "minutes" or "minute" => TimeSpan.FromMinutes(value),
            "hours" or "hour" => TimeSpan.FromHours(value),
            _ => throw new ArgumentException($"Unknown duration unit: {unit}")
        };
    }
    
    private static decimal ParsePrice(string price)
    {
        if (price.StartsWith("$"))
        {
            return decimal.Parse(price[1..]);
        }

        return decimal.Parse(price);
    }
    
    [When("I change healthcare service type price of {string} to {string} on {string}")]
    public async Task WhenIChangeThePriceOn(string serviceCode, string newPrice, string dateString)
    {
        var date = DateOnly.Parse(dateString);
        ApplicationClock.SetDate(date);

        var priceValue = ParsePrice(newPrice);
        var command = new ChangeHealthcareServiceTypePriceCommand(serviceCode, priceValue);
        await TestDispatcher.Execute(command);
    }
    
    [Then("the price history should contain exactly:")]
    public async Task ThenThePriceHistoryShouldContain(Table table)
    {
        _scenarioServiceTypeCode.ShouldNotBeNull();
        var query = new GetHealthcareServiceTypeQuery(_scenarioServiceTypeCode);
        var serviceType = await TestDispatcher.ExecuteQuery(query);

        var expectedEntries = table.Rows.Select(row => new
        {
            Price = ParsePrice(row["Price"]),
            EffectiveFrom = DateOnly.Parse(row["Effective From"]),
            EffectiveTo = string.IsNullOrEmpty(row["Effective To"]) ? (DateOnly?)null : DateOnly.Parse(row["Effective To"])
        }).ToList();

        serviceType.PriceHistory.Count.ShouldBe(expectedEntries.Count);

        for (var i = 0; i < expectedEntries.Count; i++)
        {
            var expected = expectedEntries[i];
            var actual = serviceType.PriceHistory[i];

            actual.Price.ShouldBe(expected.Price);
            actual.EffectiveFrom.ShouldBe(expected.EffectiveFrom);
            actual.EffectiveTo.ShouldBe(expected.EffectiveTo);
        }
    }
}