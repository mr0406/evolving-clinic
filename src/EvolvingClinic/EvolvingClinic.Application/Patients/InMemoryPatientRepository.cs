using EvolvingClinic.Domain.Patients;

namespace EvolvingClinic.Application.Patients;

public class InMemoryPatientRepository : IPatientRepository
{
    private static List<Patient> _patients = new();

    public Task<Patient?> GetOptional(Guid id)
    {
        var patient = _patients.SingleOrDefault(p => p.Id == id);
        
        return Task.FromResult(patient);
    }

    public Task<PatientDto> GetDto(Guid id)
    {
        var patient = _patients.SingleOrDefault(p => p.Id == id);
        
        if (patient is null)
        {
            throw new InvalidOperationException($"Patient with id {id} not found");
        }

        var snapshot = patient.CreateSnapshot();
        
        return Task.FromResult(new PatientDto(
            snapshot.Id,
            new PatientDto.PersonNameData(snapshot.Name.FirstName, snapshot.Name.LastName),
            snapshot.DateOfBirth,
            new PatientDto.PhoneNumberData(snapshot.PhoneNumber.CountryCode, snapshot.PhoneNumber.Number),
            new PatientDto.AddressData(snapshot.Address.Street, snapshot.Address.HouseNumber, snapshot.Address.Apartment, snapshot.Address.PostalCode, snapshot.Address.City)));
    }

    public Task<IReadOnlyList<PatientDto>> GetAllDtos()
    {
        var dtos = _patients.Select(p =>
        {
            var snapshot = p.CreateSnapshot();
            return new PatientDto(
                snapshot.Id,
                new PatientDto.PersonNameData(snapshot.Name.FirstName, snapshot.Name.LastName),
                snapshot.DateOfBirth,
                new PatientDto.PhoneNumberData(snapshot.PhoneNumber.CountryCode, snapshot.PhoneNumber.Number),
                new PatientDto.AddressData(snapshot.Address.Street, snapshot.Address.HouseNumber, snapshot.Address.Apartment, snapshot.Address.PostalCode, snapshot.Address.City)
            );
        }).ToList();

        return Task.FromResult<IReadOnlyList<PatientDto>>(dtos);
    }

    public Task Save(Patient patient)
    {
        var existingIndex = _patients.FindIndex(p => p.Id == patient.Id);
        
        if (existingIndex >= 0)
        {
            _patients[existingIndex] = patient;
        }
        else
        {
            _patients.Add(patient);
        }

        return Task.CompletedTask;
    }

    public static void Clear()
    {
        _patients = new();
    }
}