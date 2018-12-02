using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.DTO.Model.Client;

namespace Insurance.Service.Contracts
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetClients();

        Task<ClientDto> GetClientById(Guid id);

        Task<IEnumerable<ClientDto>> GetClientByName(string name);

        Task<ClientDto> GetClientByEmail(string email);

        Task<ClientDto> GetClientByPolicyId(Guid policyId);
    }
}