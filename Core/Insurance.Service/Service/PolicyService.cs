using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.DTO.Model.Client;
using Insurance.DTO.Model.Policy;
using Insurance.Proxy.Contracts;
using Insurance.Service.Contracts;

namespace Insurance.Service.Service
{
    public class PolicyService : IPolicyService
    {
        public readonly IHttpProxy<Policy> policyProxy;
        public readonly IHttpProxy<Client> clientProxy;

        private const string policiesServiceEndPoint = "http://www.mocky.io/v2/580891a4100000e8242b75c5";
        private const string clientsServiceEndPoint = "http://www.mocky.io/v2/5808862710000087232b75ac";

        public PolicyService(IHttpProxy<Policy> httpPolicyProxy, IHttpProxy<Client> clientProxy)
        {
            this.policyProxy = httpPolicyProxy;
            this.clientProxy = clientProxy;
        }

        public async Task<IEnumerable<PolicyDto>> GetPolicies()
        {
            IEnumerable<PolicyDto> policy = await GetPoliciesFromExternalService();

            return policy;
        }

        public async Task<PolicyDto> GetPolicyById(Guid id)
        {
            IEnumerable<PolicyDto> policies = await GetPoliciesFromExternalService();
            PolicyDto policyById = policies.FirstOrDefault(o => o.Id == id);

            return policyById;
        }

        public async Task<IEnumerable<PolicyDto>> GetPoliciesByClientName(string clientName)
        {
            List<PolicyDto> policiesByClients = new List<PolicyDto>();
            IEnumerable<ClientDto> clients = await GetClientsFromExternalService();
            IEnumerable<ClientDto> clientsDto = clients.Where(o => o.Name == clientName).ToList();
            IEnumerable<PolicyDto> policies = await GetPoliciesFromExternalService();

            foreach (var item in clientsDto)
            {
                IEnumerable<PolicyDto> policiesByClient = policies.Where(o => o.ClientId == item.Id).ToList();

                policiesByClients.AddRange(policiesByClient);
            }

            return policiesByClients;
        }

        private async Task<IEnumerable<PolicyDto>> GetPoliciesFromExternalService()
        {
            Policy policy = await this.policyProxy.GetEntityCollection(policiesServiceEndPoint);
            IEnumerable<PolicyDto> policiesDto = policy.Policies;

            return policiesDto;
        }

        private async Task<IEnumerable<ClientDto>> GetClientsFromExternalService()
        {
            Client client = await this.clientProxy.GetEntityCollection(clientsServiceEndPoint);
            IEnumerable<ClientDto> clientsDto = client.Clients;

            return clientsDto;
        }
    }
}