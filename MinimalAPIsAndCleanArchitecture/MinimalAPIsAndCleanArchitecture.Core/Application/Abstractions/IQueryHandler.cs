namespace MinimalAPIsAndCleanArchitecture.Core.Application.Abstractions;

public interface IQueryHandler<TQuery, TResult>
{
    Task<TResult> HandleAsync(TQuery query);
}
