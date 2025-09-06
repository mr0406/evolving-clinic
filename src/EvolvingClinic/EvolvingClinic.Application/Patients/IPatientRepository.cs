using EvolvingClinic.Domain.Patients;

namespace EvolvingClinic.Application.Patients;

public interface IPatientRepository
{
    Task<Patient?> GetOptional(Guid id);
    Task<PatientDto> GetDto(Guid id);
    Task<IReadOnlyList<PatientDto>> GetAllDtos();
    Task Save(Patient patient);
}