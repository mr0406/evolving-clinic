namespace EvolvingClinic.BusinessTests.Utils;

public static class TestErrorContext
{
    private static Exception? _lastException;

    public static void ClearLastError() => _lastException = null;
    public static void CaptureError(Exception ex) => _lastException = ex;
    public static Exception? GetLastError() => _lastException;
}