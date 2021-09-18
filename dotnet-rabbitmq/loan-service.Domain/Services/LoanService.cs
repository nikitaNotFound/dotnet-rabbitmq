using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using publisher.Domain.Models;
using publisher.Domain.Services.Interfaces;
using shared.Lib.Models;

namespace publisher.Domain.Services
{
    public class LoanService : ILoanService
    {
        private readonly MongoDbContext _context;

        public LoanService(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<LoanRequest> CreateLoanRequestAsync(LoanRequest loanRequest)
        {
            await _context.LoanRequests.InsertOneAsync(loanRequest);

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