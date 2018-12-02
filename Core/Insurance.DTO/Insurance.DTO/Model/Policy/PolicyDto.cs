using System;

namespace Insurance.DTO.Model.Policy
{
    public class PolicyDto
    {
        public Guid Id { get; set; }

        public decimal AmountInsured { get; set; }

        public string Email { get; set; }

        public DateTime InceptionDate { get; set; }

        public bool InstallmentPayment { get; set; }

        public Guid ClientId { get; set; }
    }
}