using System;

namespace consumer.Domain.Options
{
    public class BrokerEndpointsOptions
    {
        public Uri LoanProcessingQueue { get; set; }
    }
}