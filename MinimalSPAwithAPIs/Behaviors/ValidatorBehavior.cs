namespace MinimalSPAwithAPIs.Behaviors;

public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ValidatorBehavior<TRequest, TResponse>> _logger;

    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidatorBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Validating command {typeof(TRequest).Name}");

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(request, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        var numeroErrori = 0;

        if (failures.Any())
        {
            var errori = "Errori: ";
            foreach (var failure in failures)
            {
                numeroErrori++;
                errori += $"Errore {numeroErrori}: {failure.ErrorMessage} ";
            }
            throw new ValidationException($"Errore nella validazione dei dati in {typeof(TRequest).Name}. {numeroErrori} {errori}.");
        }

        return await next();
    }
}
