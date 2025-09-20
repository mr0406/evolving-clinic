namespace EvolvingClinic.Application.Common;

public interface IDispatcher
{
    Task<TResult> Execute<TResult>(ICommand<TResult> command);
    Task Execute(ICommand command);
    Task<TResult> ExecuteQuery<TResult>(IQuery<TResult> query);
}