namespace EvolvingClinic.Domain.Utils;

public static class ApplicationClock
{
    public static DateTime Now => _customDate ?? DateTime.Now;

    public static DateOnly Today => DateOnly.FromDateTime(Now.Date);

    private static DateTime? _customDate;

    public static void SetDate(DateTime customDate)
    {
        _customDate = customDate;
    }
    
    public static void SetDate(DateOnly customDate)
    {
        _customDate = customDate.ToDateTime(new TimeOnly());
    }
    
    public static void Reset()
    {
        _customDate = null;
    }
}