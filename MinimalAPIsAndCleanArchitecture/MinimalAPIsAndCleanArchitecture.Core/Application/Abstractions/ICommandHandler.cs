namespace MinimalAPIsAndCleanArchitecture.Core.Application.Abstractions;

public interface ICommandHandler<TCommand, TResult>
{
    Task<TResult> HandleAsync(TCommand command);
}
