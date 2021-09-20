using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using publisher.Domain.Models;
using publisher.Domain.Options;
using publisher.Domain.Services.Interfaces;
using shared.Lib.BrokerModels;
using shared.Lib.Models;

namespace publisher.Domain.Services
{
    public class LoanService : ILoanService
    {
        private readonly MongoDbContext _context;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly BrokerEndpointsOptions _brokerEndpoints;

        public LoanService(
            MongoDbContext context,
            ISendEndpointProvider sendEndpointProvider,
            IOptions<BrokerEndpointsOptions> brokerEndpointsOptions)
        {
            _context = context;
            _sendEndpointProvider = sendEndpointProvider;
            _brokerEndpoints = brokerEndpointsOptions.Value;
        }

        public async Task<LoanRequest> CreateLoanRequestAsync(LoanRequest loanRequest)
        {
            await _context.LoanRequests.InsertOneAsync(loanRequest);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(_brokerEndpoints.LoanProcessingQueue);
            await endpoint.Send(loanRequest.Adapt<LoanRequestBroker>());

            return loanRequest;
        }

        public async Task<Paged<LoanRequest>> GetLoanRequestsAsync(PagedParams pagedParams)
        {
            IList<LoanRequest> loans = await _context.LoanRequests
                .Find(x => true)
                .Skip(pagedParams.PageNumber * pagedParams.PageSize)
                .Limit(pagedParams.PageSize)
                .ToListAsync();

            long totalCount = await _context.LoanRequests
                .Find(x => true)
                .CountDocumentsAsync();

            return new Paged<LoanRequest>
            {
                Items = loans,
                HasMore = pagedParams.PageSize * pagedParams.PageNumber < totalCount
            };
        }
    }
}