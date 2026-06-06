using Microsoft.Extensions.DependencyInjection;

namespace SweetMedical.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // TODO: register application services (e.g. MediatR, FluentValidation, mappers).
        return services;
    }
}
