using System.Threading.Tasks;
using consumer.Domain.Models;

namespace consumer.Domain.Services.Interfaces
{
    public interface ILoanProcessingService
    {
        Task<LoanProcessingInfo> ProcessAsync(LoanProcessingInfo request);

        Task<LoanProcessingInfo> SaveProcessingInfoAsync(LoanProcessingInfo loanProcessingInfo);
    }
}