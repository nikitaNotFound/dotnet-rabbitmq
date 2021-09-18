using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Mapster;

using publisher.Api.Models;

using publisher.Domain.Models;
using publisher.Domain.Services.Interfaces;

using shared.Lib.Models;

namespace publisher.Api.Controllers
{
    [ApiController]
    [Route("api/loans")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoanRequestAsync(
            [FromBody] LoanRequestDTO loanRequestDto)
        {
            LoanRequest loanRequest = await _loanService.CreateLoanRequestAsync(
                loanRequestDto.Adapt<LoanRequest>());

            return Ok(loanRequest.Adapt<LoanRequestDTO>());
        }

        [HttpGet]
        public async Task<IActionResult> GetLoanRequestsAsync(
            [FromQuery] PagedParams pagedParams)
        {
            Paged<LoanRequest> loanRequests = await _loanService.GetLoanRequestsAsync(
                pagedParams);

            return Ok(loanRequests.Adapt<LoanRequestDTO>());
        }
    }
}