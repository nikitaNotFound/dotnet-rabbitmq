using consumer.Domain.Models.Enums;

namespace consumer.Domain.Models
{
    public class LoanProcessingInfo
    {
        public int Id { get; set; }

        public int LoanRequestId { get; set; }

        public TaskStatus Status { get; set; }

        public LoanProcessingResult Result { get; set; }
    }
}