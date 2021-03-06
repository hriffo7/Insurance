using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.DTO.Model.Policy;

namespace Insurance.Service.Contracts
{
    public interface IPolicyService
    {
        Task<IEnumerable<PolicyDto>> GetPoliciesByClientName(string clientName);
    }
}