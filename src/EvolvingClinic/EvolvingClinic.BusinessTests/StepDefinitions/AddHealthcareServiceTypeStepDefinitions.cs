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
    private AddHealthcareServiceTypeData? _scenarioServiceTypeData;

    [When("I add healthcare service type {string} with code {string}, duration {string} and price {string}")]
    public async Task WhenIAddHealthcareServiceType(string name, string code, string duration, string price)
    {
        var timeSpan = ParseDuration(duration);
        var priceValue = ParsePrice(price);
        _scenarioServiceTypeData = new(name, code, timeSpan, priceValue);

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

    [Then("the healthcare service type should be added with the correct data")]
    public async Task ThenTheHealthcareServiceTypeShouldBeAddedWithTheCorrectData()
    {
        _scenarioServiceTypeCode.ShouldNotBeNull();
        _scenarioServiceTypeData.ShouldNotBeNull();

        var query = new GetHealthcareServiceTypeQuery(_scenarioServiceTypeCode);
        var serviceType = await _dispatcher.ExecuteQuery(query);

        serviceType.ShouldNotBeNull();
        serviceType.Code.ShouldBe(_scenarioServiceTypeData.Code);
        serviceType.Name.ShouldBe(_scenarioServiceTypeData.Name);
        serviceType.Duration.ShouldBe(_scenarioServiceTypeData.Duration);
        serviceType.Price.ShouldBe(_scenarioServiceTypeData.Price);
    }

    [Then("there should be {int} healthcare service types in the system")]
    public async Task ThenThereShouldBeHealthcareServiceTypesInTheSystem(int expectedCount)
    {
        var repository = new InMemoryHealthcareServiceTypeRepository();
        var serviceTypes = await repository.GetAllDtos();
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

    private record AddHealthcareServiceTypeData(
        string Name,
        string Code,
        TimeSpan Duration,
        decimal Price);
}