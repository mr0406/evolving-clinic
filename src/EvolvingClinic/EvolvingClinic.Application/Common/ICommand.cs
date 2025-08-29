namespace EvolvingClinic.Application.Common;

public interface ICommand
{
}

public interface ICommand<TResult> : ICommand
{
}

public interface IQuery<TResult>
{
}