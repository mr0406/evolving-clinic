using EvolvingClinic.Domain.Doctors;

namespace EvolvingClinic.Application.Doctors;

public interface IDoctorRepository
{
    Task<Doctor?> GetOptional(string code);
    Task<DoctorDto> GetDto(string code);
    Task<IReadOnlyList<DoctorDto>> GetAllDtos();
    Task<IReadOnlyList<string>> GetAllCodes();
    Task Save(Doctor doctor);
}