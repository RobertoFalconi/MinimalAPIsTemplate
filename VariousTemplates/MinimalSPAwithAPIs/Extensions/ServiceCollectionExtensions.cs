namespace MinimalSPAwithAPIs.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services, Assembly assembly)
    {
        var validatorType = typeof(IValidator<>);

        var validators = assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Select(t => new
            {
                ServiceType = t.GetInterfaces().FirstOrDefault(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == validatorType),
                ImplementationType = t
            })
            .Where(x => x.ServiceType != null);

        foreach (var validator in validators)
        {
            services.AddScoped(validator.ServiceType, validator.ImplementationType);
        }

        return services;
    }
}