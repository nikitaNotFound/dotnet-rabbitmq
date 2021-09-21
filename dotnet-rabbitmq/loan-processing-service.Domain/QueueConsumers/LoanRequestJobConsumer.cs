using System.Threading.Tasks;
using consumer.Domain.Models;
using consumer.Domain.Services.Interfaces;
using Mapster;
using MassTransit.JobService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using shared.Lib.BrokerModels;
using TaskStatus = consumer.Domain.Models.Enums.TaskStatus;

namespace consumer.Domain.QueueConsumers
{
    public class LoanRequestJobConsumer : IJobConsumer<LoanRequestBroker>
    {
        private readonly ILoanProcessingService _processingService;
        private readonly ILogger<LoanRequestJobConsumer> _logger;

        public LoanRequestJobConsumer(
            ILoanProcessingService processingService,
            ILogger<LoanRequestJobConsumer> logger)
        {
            _processingService = processingService;
            _logger = logger;
        }

        public async Task Run(JobContext<LoanRequestBroker> context)
        {
            _logger.LogInformation($"{nameof(LoanRequestJobConsumer)}: start processing loan request id = {context.Job.Id}");

            var processingInfo = new LoanProcessingInfo
            {
                Status = TaskStatus.InProgress,
                LoanRequest = context.Job.Adapt<LoanRequest>()
            };

            processingInfo = await _processingService.SaveProcessingInfoAsync(processingInfo);

            processingInfo = await _processingService.ProcessAsync(processingInfo);

            processingInfo = await _processingService.SaveProcessingInfoAsync(processingInfo);

            _logger.LogInformation($"{nameof(LoanRequestJobConsumer)}: end processing loan request id = {context.Job.Id}" +
                                   $"\nResult: {JsonConvert.SerializeObject(processingInfo)}");
        }
    }
}