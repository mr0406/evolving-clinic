using EvolvingClinic.Domain.Patients;

namespace EvolvingClinic.Application.Patients;

public interface IPatientRepository
{
    Task<Patient?> GetOptional(Guid id);
    Task<PatientDto> GetDto(Guid id);
    Task Save(Patient patient);
}