using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.DTO.Model.Client;
using Insurance.DTO.Model.Policy;
using Insurance.Proxy.Contracts;
using Insurance.Service.Contracts;
using Microsoft.Extensions.Configuration;

namespace Insurance.Service.Service
{
    public class PolicyService : IPolicyService
    {
        public readonly IHttpProxy<Policy> policyProxy;
        public readonly IHttpProxy<Client> clientProxy;
        public IConfiguration configuration;

        public PolicyService(IHttpProxy<Policy> httpPolicyProxy, IHttpProxy<Client> clientProxy, IConfiguration configuration)
        {
            this.policyProxy = httpPolicyProxy;
            this.clientProxy = clientProxy;
            this.configuration = configuration;
        }

        public async Task<IEnumerable<PolicyDto>> GetPoliciesByClientName(string clientName)
        {
            List<PolicyDto> policiesByClients = new List<PolicyDto>();
            IEnumerable<ClientDto> clients = await GetClientsFromExternalService();
            IEnumerable<ClientDto> clientsDto = clients.Where(o => o.Name.ToLower() == clientName.ToLower());
            IEnumerable<PolicyDto> policies = await GetPoliciesFromExternalService();

            foreach (var item in clientsDto)
            {
                policiesByClients.AddRange(policies.Where(o => o.ClientId == item.Id));
            }

            return policiesByClients;
        }

        public async Task<IEnumerable<PolicyDto>> GetPoliciesFromExternalService()
        {
            Policy policy = await this.policyProxy.GetEntityCollection(this.configuration["policiesServiceEndPoint"]);
            IEnumerable<PolicyDto> policiesDto = policy.Policies;

            return policiesDto;
        }

        public async Task<IEnumerable<ClientDto>> GetClientsFromExternalService()
        {
            Client client = await this.clientProxy.GetEntityCollection(this.configuration["clientsServiceEndPoint"]);
            IEnumerable<ClientDto> clientsDto = client.Clients;

            return clientsDto;
        }
    }
}