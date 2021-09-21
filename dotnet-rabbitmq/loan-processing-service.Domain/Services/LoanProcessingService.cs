using System;
using System.Threading.Tasks;
using consumer.Domain.Models;
using consumer.Domain.Models.Enums;
using consumer.Domain.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using TaskStatus = consumer.Domain.Models.Enums.TaskStatus;

namespace consumer.Domain.Services
{
    public class LoanProcessingService : ILoanProcessingService
    {
        private readonly MongoDbContext _context;

        public LoanProcessingService(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<LoanProcessingInfo> ProcessAsync(LoanProcessingInfo info)
        {
            try
            {
                if (new Random().Next(1, 10) >= 8)
                {
                    throw new Exception();
                }

                info.Result = info.LoanRequest.MonthlyRevenue > 10000
                    ? LoanProcessingResult.Ok
                    : LoanProcessingResult.NotEnoughMonthlyRevenue;

                info.Status = TaskStatus.Completed;
            }
            catch
            {
                info.Status = TaskStatus.Failed;
            }
            finally
            {
                // simulate work
                await Task.Delay(2000);
            }

            return info;
        }

        public async Task<LoanProcessingInfo> SaveProcessingInfoAsync(LoanProcessingInfo loanProcessingInfo)
        {
            if (loanProcessingInfo.Id == Guid.Empty)
            {
                await _context.LoanProcessingInfos.InsertOneAsync(loanProcessingInfo);

                return loanProcessingInfo;
            }

            FilterDefinition<LoanProcessingInfo> filter =
                Builders<LoanProcessingInfo>.Filter.Where(x => x.Id == loanProcessingInfo.Id);

            await _context.LoanProcessingInfos.ReplaceOneAsync(filter, loanProcessingInfo);

            return loanProcessingInfo;
        }
    }
}