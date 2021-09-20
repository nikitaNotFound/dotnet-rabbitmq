using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using shared.Lib.BrokerModels;

namespace consumer.Domain.QueueConsumers
{
    public class LoanRequestConsumer : IConsumer<LoanRequestBroker>
    {
        private readonly ILogger<LoanRequestConsumer> _logger;

        public LoanRequestConsumer(
            ILogger<LoanRequestConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<LoanRequestBroker> context)
        {
            _logger.LogInformation($"{nameof(LoanRequestConsumer)}: start processing loan request id = {context.Message.Id}");

            return Task.CompletedTask;
        }
    }
}