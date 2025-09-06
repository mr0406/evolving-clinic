using EvolvingClinic.Application.Appointments;
using EvolvingClinic.Application.Patients;
using Reqnroll;

namespace EvolvingClinic.BusinessTests.Hooks;

[Binding]
public class TestHooks
{
    [BeforeScenario]
    public void BeforeScenario()
    {
        InMemoryDailyAppointmentScheduleRepository.Clear();
        InMemoryPatientRepository.Clear();
    }
}