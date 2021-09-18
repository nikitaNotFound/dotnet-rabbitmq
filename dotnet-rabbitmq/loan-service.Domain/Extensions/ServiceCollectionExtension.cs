using System;

using Microsoft.Extensions.DependencyInjection;

using publisher.Domain.Options;
using publisher.Domain.Services;
using publisher.Domain.Services.Interfaces;

namespace publisher.Domain.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDomainServices(
            this IServiceCollection services,
            Action<MongoDbOptions> configure)
        {
            services.Configure(configure);

            services.AddScoped<ILoanService, LoanService>();

            services.AddScoped<MongoDbContext>();

            return services;
        }
    }
}