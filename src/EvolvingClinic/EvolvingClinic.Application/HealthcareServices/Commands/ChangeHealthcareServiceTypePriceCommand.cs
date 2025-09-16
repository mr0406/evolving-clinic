using EvolvingClinic.Application.Common;
using EvolvingClinic.Domain.Shared;

namespace EvolvingClinic.Application.HealthcareServices.Commands;

public record ChangeHealthcareServiceTypePriceCommand(
    string ServiceCode,
    decimal NewPrice
) : ICommand;

public class ChangeHealthcareServiceTypePriceCommandHandler(IHealthcareServiceTypeRepository repository)
    : ICommandHandler<ChangeHealthcareServiceTypePriceCommand>
{
    public async Task Handle(ChangeHealthcareServiceTypePriceCommand command)
    {
        var serviceType = await repository.GetOptional(command.ServiceCode);
        if (serviceType == null)
        {
            throw new InvalidOperationException($"Healthcare service type with code '{command.ServiceCode}' not found");
        }

        var newPrice = new Money(command.NewPrice);
        serviceType.ChangePrice(newPrice);

        await repository.Save(serviceType);
    }
}