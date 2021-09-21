using System;

namespace consumer.Domain.Models
{
    public class LoanRequest
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ReasonDescription { get; set; }

        public decimal MonthlyRevenue { get; set; }

        public string Address { get; set; }
    }
}