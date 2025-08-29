namespace EvolvingClinic.Application.Common;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task Handle(TCommand command);
}

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> Handle(TCommand command);
}

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query);
}