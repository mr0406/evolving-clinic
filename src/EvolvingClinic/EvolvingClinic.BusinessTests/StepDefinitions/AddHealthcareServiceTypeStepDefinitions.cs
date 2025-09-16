using EvolvingClinic.Application.Common;
using EvolvingClinic.Application.HealthcareServices;
using EvolvingClinic.Application.HealthcareServices.Commands;
using EvolvingClinic.Application.HealthcareServices.Queries;
using Reqnroll;
using Shouldly;

namespace EvolvingClinic.BusinessTests.StepDefinitions;

[Binding]
public sealed class AddHealthcareServiceTypeStepDefinitions
{
    private readonly Dispatcher _dispatcher = new();
    private string? _scenarioServiceTypeCode;
    private Exception? _scenarioException;

    [When("I add healthcare service type {string} with code {string}, duration {string} and price {string}")]
    public async Task WhenIAddHealthcareServiceType(string name, string code, string duration, string price)
    {
        var timeSpan = ParseDuration(duration);
        var priceValue = ParsePrice(price);

        try
        {
            await AddHealthcareServiceType(name, code, timeSpan, priceValue);
            _scenarioServiceTypeCode = code;
        }
        catch (Exception ex)
        {
            _scenarioException = ex;
            _scenarioServiceTypeCode = null;
        }
    }

    [Given("I add healthcare service type {string} with code {string}, duration {string} and price {string}")]
    [Given("I have added healthcare service type {string} with code {string}, duration {string} and price {string}")]
    [Given("healthcare service type {string} with code {string}, duration {string} and price {string} exists")]
    [Given("I have a healthcare service type {string} with code {string}, duration {string}, and price {string}")]
    public async Task GivenIHaveAddedHealthcareServiceType(string name, string code, string duration, string price)
    {
        var timeSpan = ParseDuration(duration);
        var priceValue = ParsePrice(price);
        await AddHealthcareServiceType(name, code, timeSpan, priceValue);
    }

    [Then("the healthcare service type should be added successfully")]
    public void ThenTheHealthcareServiceTypeShouldBeAddedSuccessfully()
    {
        _scenarioServiceTypeCode.ShouldNotBeNull();
        _scenarioServiceTypeCode.ShouldNotBeNullOrWhiteSpace();
        _scenarioException.ShouldBeNull();
    }

    [Then("the added healthcare service type should be:")]
    public async Task ThenTheAddedHealthcareServiceTypeShouldBe(Table table)
    {
        _scenarioServiceTypeCode.ShouldNotBeNull();

        var query = new GetHealthcareServiceTypeQuery(_scenarioServiceTypeCode);
        var serviceType = await _dispatcher.ExecuteQuery(query);

        serviceType.ShouldNotBeNull();

        var expectedRow = table.Rows[0];

        serviceType.Name.ShouldBe(expectedRow["Healthcare Service Name"]);
        serviceType.Code.ShouldBe(expectedRow["Code"]);
        serviceType.Duration.ShouldBe(ParseDuration(expectedRow["Duration"]));
        serviceType.Price.ShouldBe(ParsePrice(expectedRow["Current Price"]));

        // Validate price history
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
        var serviceTypes = await _dispatcher.ExecuteQuery(query);
        serviceTypes.Count.ShouldBe(expectedCount);
    }

    private async Task AddHealthcareServiceType(string name, string code, TimeSpan duration, decimal price)
    {
        var command = new AddHealthcareServiceTypeCommand(name, code, duration, price);
        await _dispatcher.Execute(command);
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

    [When("I change healthcare service type price of {string} to {string}")]
    public async Task WhenIChangeThePrice(string serviceCode, string newPrice)
    {
        var priceValue = ParsePrice(newPrice);
        var command = new ChangeHealthcareServiceTypePriceCommand(serviceCode, priceValue);
        await _dispatcher.Execute(command);
    }

    [Then("the current price should be {string}")]
    public async Task ThenTheCurrentPriceShouldBe(string expectedPrice)
    {
        _scenarioServiceTypeCode.ShouldNotBeNull();
        var query = new GetHealthcareServiceTypeQuery(_scenarioServiceTypeCode);
        var serviceType = await _dispatcher.ExecuteQuery(query);

        var expectedPriceValue = ParsePrice(expectedPrice);
        serviceType.Price.ShouldBe(expectedPriceValue);
    }

    [Then("the price history should contain exactly:")]
    public async Task ThenThePriceHistoryShouldContain(Table table)
    {
        _scenarioServiceTypeCode.ShouldNotBeNull();
        var query = new GetHealthcareServiceTypeQuery(_scenarioServiceTypeCode);
        var serviceType = await _dispatcher.ExecuteQuery(query);

        var expectedEntries = table.Rows.Select(row => new
        {
            Price = ParsePrice(row["Price"]),
            EffectiveFrom = DateOnly.Parse(row["Effective From"]),
            EffectiveTo = string.IsNullOrEmpty(row["Effective To"]) ? (DateOnly?)null : DateOnly.Parse(row["Effective To"])
        }).ToList();

        serviceType.PriceHistory.Count.ShouldBe(expectedEntries.Count);

        for (int i = 0; i < expectedEntries.Count; i++)
        {
            var expected = expectedEntries[i];
            var actual = serviceType.PriceHistory[i];

            actual.Price.ShouldBe(expected.Price);
            actual.EffectiveFrom.ShouldBe(expected.EffectiveFrom);
            actual.EffectiveTo.ShouldBe(expected.EffectiveTo);
        }
    }

}