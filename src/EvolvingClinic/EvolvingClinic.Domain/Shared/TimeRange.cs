namespace EvolvingClinic.Domain.Shared;

public record TimeRange
{
    public TimeOnly Start { get; }
    public TimeOnly End { get; }

    public TimeRange(TimeOnly start, TimeOnly end)
    {
        if (end <= start)
        {
            throw new ArgumentException("End time must be after start time");
        }

        Start = start;
        End = end;
    }
}