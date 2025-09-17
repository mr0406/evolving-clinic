using EvolvingClinic.Domain.Doctors;

namespace EvolvingClinic.Application.Doctors;

public class InMemoryDoctorRepository : IDoctorRepository
{
    private static List<Doctor> _doctors = new();

    public Task<Doctor?> GetOptional(string code)
    {
        var doctor = _doctors.SingleOrDefault(d => d.Code == code);

        return Task.FromResult(doctor);
    }

    public Task<DoctorDto> GetDto(string code)
    {
        var doctor = _doctors.SingleOrDefault(d => d.Code == code);

        if (doctor is null)
        {
            throw new InvalidOperationException($"Doctor with code {code} not found");
        }

        var snapshot = doctor.CreateSnapshot();

        return Task.FromResult(new DoctorDto(
            snapshot.Code,
            new DoctorDto.PersonNameData(snapshot.Name.FirstName, snapshot.Name.LastName)));
    }

    public Task<IReadOnlyList<DoctorDto>> GetAllDtos()
    {
        var dtos = _doctors.Select(d =>
        {
            var snapshot = d.CreateSnapshot();
            return new DoctorDto(
                snapshot.Code,
                new DoctorDto.PersonNameData(snapshot.Name.FirstName, snapshot.Name.LastName));
        }).ToList();

        return Task.FromResult<IReadOnlyList<DoctorDto>>(dtos);
    }

    public Task<IReadOnlyList<string>> GetAllCodes()
    {
        var codes = _doctors
            .Select(d => d.Code)
            .ToList();

        return Task.FromResult<IReadOnlyList<string>>(codes);
    }

    public Task Save(Doctor doctor)
    {
        var existingIndex = _doctors.FindIndex(d => d.Code == doctor.Code);

        if (existingIndex >= 0)
        {
            _doctors[existingIndex] = doctor;
        }
        else
        {
            _doctors.Add(doctor);
        }

        return Task.CompletedTask;
    }

    public static void Clear()
    {
        _doctors = new();
    }
}