using System;
using consumer.Domain.Models.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace consumer.Domain.Models
{
    public class LoanProcessingInfo
    {
        [BsonId]
        public Guid Id { get; set; }

        public TaskStatus Status { get; set; }

        public LoanProcessingResult Result { get; set; }

        public LoanRequest LoanRequest { get; set; }
    }
}