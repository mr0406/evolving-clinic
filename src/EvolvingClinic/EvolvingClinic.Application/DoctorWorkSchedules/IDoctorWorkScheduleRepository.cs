using EvolvingClinic.Domain.DoctorWorkSchedules;

namespace EvolvingClinic.Application.DoctorWorkSchedules;

public interface IDoctorWorkScheduleRepository
{
    Task<DoctorWorkSchedule?> GetOptional(string doctorCode);
    Task<DoctorWorkScheduleDto?> GetDtoOptional(string doctorCode);
    Task Save(DoctorWorkSchedule schedule);
}