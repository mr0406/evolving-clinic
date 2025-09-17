namespace EvolvingClinic.Application.Doctors;

public record DoctorDto(
    string Code,
    DoctorDto.PersonNameData Name)
{
    public record PersonNameData(string FirstName, string LastName);
}