using EvolvingClinic.Application.Common;
using EvolvingClinic.Domain.HealthcareServices;

namespace EvolvingClinic.Application.HealthcareServices.Commands;

public record AddHealthcareServiceTypeCommand(
    string Name,
    string Code,
    TimeSpan Duration
) : ICommand<Guid>;

public class AddHealthcareServiceTypeCommandHandler(IHealthcareServiceTypeRepository repository)
    : ICommandHandler<AddHealthcareServiceTypeCommand, Guid>
{
    public async Task<Guid> Handle(AddHealthcareServiceTypeCommand command)
    {
        var existingNames = await repository.GetAllNames();
        var existingCodes = await repository.GetAllCodes();

        var healthcareServiceType = HealthcareServiceType.Create(
            command.Name,
            command.Code,
            command.Duration,
            existingNames.ToList(),
            existingCodes.ToList());

        await repository.Save(healthcareServiceType);

        return healthcareServiceType.Id;
    }
}