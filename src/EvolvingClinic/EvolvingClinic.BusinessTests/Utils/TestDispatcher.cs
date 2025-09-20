using EvolvingClinic.Application.Common;

namespace EvolvingClinic.BusinessTests.Utils;

public static class TestDispatcher
{
    private static readonly IDispatcher InnerDispatcher = new Dispatcher();

    public static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        TestErrorContext.ClearLastError();

        try
        {
            return await InnerDispatcher.Execute(command);
        }
        catch (Exception ex)
        {
            TestErrorContext.CaptureError(ex);
            return default!;
        }
    }

    public static async Task Execute(ICommand command)
    {
        TestErrorContext.ClearLastError();

        try
        {
            await InnerDispatcher.Execute(command);
        }
        catch (Exception ex)
        {
            TestErrorContext.CaptureError(ex);
        }
    }

    public static async Task<TResult> ExecuteQuery<TResult>(IQuery<TResult> query)
    {
        TestErrorContext.ClearLastError();

        try
        {
            return await InnerDispatcher.ExecuteQuery(query);
        }
        catch (Exception ex)
        {
            TestErrorContext.CaptureError(ex);
            return default!;
        }
    }
}