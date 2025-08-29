namespace EvolvingClinic.Domain.Appointments;

public readonly record struct AppointmentTimeSlot
{
    public DateOnly Date { get; }
    public TimeOnly StartTime { get; }
    public TimeOnly EndTime { get; }

    public AppointmentTimeSlot(DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        if (startTime >= endTime)
        {
            throw new ArgumentException("Start time must be before end time");
        }

        if ((endTime - startTime).TotalMinutes < 15)
        {
            throw new ArgumentException("Appointment must be at least 15 minutes long");
        }

        Date = date;
        StartTime = startTime;
        EndTime = endTime;
    }
    
    public bool OverlapsWith(AppointmentTimeSlot other)
    {
        if (Date != other.Date)
        {
            return false;
        }
    
        return StartTime < other.EndTime && EndTime > other.StartTime;
    }

    public TimeSpan Duration => EndTime - StartTime;
    public DateTime StartDateTime => Date.ToDateTime(StartTime);
    public DateTime EndDateTime => Date.ToDateTime(EndTime);
}