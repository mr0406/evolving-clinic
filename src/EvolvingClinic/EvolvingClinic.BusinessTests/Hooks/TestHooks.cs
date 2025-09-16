using EvolvingClinic.Application.Appointments;
using EvolvingClinic.Application.HealthcareServices;
using EvolvingClinic.Application.Patients;
using EvolvingClinic.Domain.Utils;
using Reqnroll;

namespace EvolvingClinic.BusinessTests.Hooks;

[Binding]
public class TestHooks
{
    [BeforeScenario]
    public void BeforeScenario()
    {
        ApplicationClock.Reset();
        
        InMemoryPatientRepository.Clear();
        InMemoryHealthcareServiceTypeRepository.Clear();
        InMemoryDailyAppointmentScheduleRepository.Clear();
    }
}