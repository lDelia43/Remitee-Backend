using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SweetMedical.Infrastructure.DependencyInjection;

/// <summary>
/// Composition root for the Infrastructure layer. Register persistence,
/// external services, repositories, etc. here.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // TODO: register infrastructure services (e.g. DbContext, repositories, clients).
        return services;
    }
}
