using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using MassTransit;
using MassTransit.Contracts.JobService;
using Microsoft.Extensions.Logging;
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
        private readonly IRequestClient<LoanRequestBroker> _requestClient;
        private readonly ILogger<LoanService> _logger;

        public LoanService(
            MongoDbContext context,
            IRequestClient<LoanRequestBroker> requestClient,
            ILogger<LoanService> logger)
        {
            _context = context;
            _requestClient = requestClient;
            _logger = logger;
        }

        public async Task<LoanRequest> CreateLoanRequestAsync(LoanRequest loanRequest)
        {
            loanRequest.Id ??= Guid.NewGuid().ToString();
            await _context.LoanRequests.InsertOneAsync(loanRequest);

            Response<JobSubmissionAccepted> response = await _requestClient.GetResponse<JobSubmissionAccepted>(
                loanRequest.Adapt<LoanRequestBroker>());

            _logger.LogInformation($"Job with id = {response.Message.JobId} scheduled.");

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