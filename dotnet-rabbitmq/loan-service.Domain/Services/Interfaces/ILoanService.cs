using System.Threading.Tasks;

using publisher.Domain.Models;
using shared.Lib.Models;

namespace publisher.Domain.Services.Interfaces
{
    public interface ILoanService
    {
        Task<LoanRequest> CreateLoanRequestAsync(LoanRequest loanRequest);

        Task<Paged<LoanRequest>> GetLoanRequestsAsync(PagedParams pagedParams);
    }
}