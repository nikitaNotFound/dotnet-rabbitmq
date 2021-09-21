using System;
using consumer.Domain.Options;
using consumer.Domain.Services;
using consumer.Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace consumer.Domain.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDomainServices(
            this IServiceCollection services,
            Action<MongoDbOptions> configureMongo)
        {
            services.Configure(configureMongo);

            services.AddScoped<MongoDbContext>();

            services.AddScoped<ILoanProcessingService, LoanProcessingService>();

            return services;
        }
    }
}