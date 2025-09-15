using EvolvingClinic.Application.Common;
using EvolvingClinic.Domain.HealthcareServices;
using EvolvingClinic.Domain.Shared;

namespace EvolvingClinic.Application.HealthcareServices.Commands;

public record AddHealthcareServiceTypeCommand(
    string Name,
    string Code,
    TimeSpan Duration,
    decimal Price
) : ICommand;

public class AddHealthcareServiceTypeCommandHandler(IHealthcareServiceTypeRepository repository)
    : ICommandHandler<AddHealthcareServiceTypeCommand>
{
    public async Task Handle(AddHealthcareServiceTypeCommand command)
    {
        var price = new Money(command.Price);
        
        var existingNames = await repository.GetAllNames();
        var existingCodes = await repository.GetAllCodes();

        var healthcareServiceType = HealthcareServiceType.Create(
            command.Name,
            command.Code,
            command.Duration,
            price,
            existingNames.ToList(),
            existingCodes.ToList());

        await repository.Save(healthcareServiceType);
    }
}