using System;
using System.Collections.Generic;

namespace Insurance.DTO.Model.Policy
{
    public class Policy
    {
        public ICollection<PolicyDto> Policies { get; set; }
    }
}