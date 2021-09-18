namespace publisher.Api.Models
{
    public class LoanRequestDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ReasonDescription { get; set; }

        public decimal MonthlyRevenue { get; set; }

        public string Address { get; set; }
    }
}