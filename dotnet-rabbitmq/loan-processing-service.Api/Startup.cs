using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using consumer.Domain.Extensions;
using consumer.Domain.Options;
using consumer.Domain.QueueConsumers;
using GreenPipes.Configurators;
using GreenPipes.Policies;
using MassTransit;
using MassTransit.Definition;
using MassTransit.JobService;
using MassTransit.JobService.Components.StateMachines;
using MassTransit.JobService.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using shared.Lib.BrokerModels;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace consumer.Api
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "loan-processing-service.Api", Version = "v1"});
            });

            services.AddDomainServices(_config.GetSection("MongoDb").Bind);

            services.AddMassTransit(x =>
            {
                x.AddDelayedMessageScheduler();

                x.AddConsumer<LoanRequestJobConsumer>(cfg =>
                {
                    cfg.Options<JobOptions<LoanRequestBroker>>(options => options
                        .SetJobTimeout(TimeSpan.FromSeconds(1))
                        .SetConcurrentJobLimit(3));
                });

                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseDelayedMessageScheduler();

                    var options = new ServiceInstanceOptions()
                        .SetEndpointNameFormatter(context.GetService<IEndpointNameFormatter>() ?? KebabCaseEndpointNameFormatter.Instance);

                    cfg.ServiceInstance(options, instance =>
                    {
                        instance.ConfigureJobServiceEndpoints(js =>
                        {
                            js.SagaPartitionCount = 1;
                            js.FinalizeCompleted = true;
                        });

                        instance.ConfigureEndpoints(context);
                    });
                });
            });
            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "loan-processing-service.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}