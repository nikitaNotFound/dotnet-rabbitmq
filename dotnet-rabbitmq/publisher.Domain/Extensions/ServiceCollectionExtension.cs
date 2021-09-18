using Microsoft.Extensions.DependencyInjection;

namespace publisher.Domain.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPublisherDomain(
            this IServiceCollection services)
        {
            return services;
        }
    }
}