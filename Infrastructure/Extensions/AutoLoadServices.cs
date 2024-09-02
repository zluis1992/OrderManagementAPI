using Domain.Ports;
using Domain.Services;
using Infrastructure.Adapters;
using Infrastructure.Adapters.Repositories;
using Infrastructure.Adapters.Services;
using Infrastructure.Ports;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class AutoLoadServices
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        // generic repository
        services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));

        // unit of work
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        // all services with infrastructure service attribute, we can also do this "by convention",
        // naming services with suffix "Service" if decide to remove the domain service decorator
        var _servicesInfrastructure = AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly =>
            {
                return (assembly.FullName is null) || assembly.FullName.Contains("Infrastructure", StringComparison.InvariantCulture);
            })
            .SelectMany(s => s.GetTypes())
            .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(InfrastructureServiceAttribute)));

        // all services with domain service attribute, we can also do this "by convention",
        // naming services with suffix "Service" if decide to remove the domain service decorator
        var _servicesDomain = AppDomain.CurrentDomain.GetAssemblies()
              .Where(assembly =>
              {
                  return (assembly.FullName is null) || assembly.FullName.Contains("Domain", StringComparison.InvariantCulture);
              })
              .SelectMany(s => s.GetTypes())
              .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(DomainServiceAttribute)));

        // Ditto, but repositories
        var _repositories = AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly =>
            {
                return (assembly.FullName is null) || assembly.FullName.Contains("Infrastructure", StringComparison.InvariantCulture);
            })
            .SelectMany(s => s.GetTypes())
            .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(RepositoryAttribute)));

        // svc Infra
        foreach (var service in _servicesInfrastructure)
        {
            Type iface = service.GetInterfaces().Single();
            services.AddTransient(iface, service);
        }

        // svc Domain
        foreach (var service in _servicesDomain)
        {
            services.AddTransient(service);
        }

        // repos
        foreach (var repo in _repositories)
        {
            Type iface = repo.GetInterfaces().Single();
            services.AddTransient(iface, repo);
        }

        return services;
    }
}
