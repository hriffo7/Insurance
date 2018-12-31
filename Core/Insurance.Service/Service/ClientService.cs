using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Insurance.Proxy.Contracts;
using Insurance.DTO.Model.Client;
using Insurance.DTO.Model.Policy;
using Insurance.Service.Contracts;
using Microsoft.Extensions.Configuration;

namespace Insurance.Service.Service
{
    public class ClientService : IClientService
    {
        public readonly IHttpProxy<Client> clientProxy;
        public readonly IHttpProxy<Policy> policyProxy;
        public IConfiguration configuration;

        public ClientService(IHttpProxy<Client> httpClientProxy, IHttpProxy<Policy> policyProxy, IConfiguration configuration)
        {
            this.clientProxy = httpClientProxy;
            this.policyProxy = policyProxy;
            this.configuration = configuration;
        }

        public async Task<ClientDto> GetClientById(Guid id)
        {
            IEnumerable<ClientDto> clients = await GetClientsFromExternalService();
            ClientDto clientsById = clients?.FirstOrDefault(o => o.Id == id);

            return clientsById;
        }

        public async Task<IEnumerable<ClientDto>> GetClientByName(string name)
        {
            IEnumerable<ClientDto> clients = await GetClientsFromExternalService();
            IEnumerable<ClientDto> clientsByName = clients.Where(o => o.Name.ToLower() == name.ToLower()).ToList();

            return clientsByName;
        }

        public async Task<ClientDto> GetClientByPolicyId(Guid policyId)
        {
            IEnumerable<PolicyDto> policies = await GetPoliciesFromExternalService();
            PolicyDto policy = policies.FirstOrDefault(o => o.Id == policyId);
            IEnumerable<ClientDto> clients = await GetClientsFromExternalService();
            ClientDto clientsByPolicy = clients.FirstOrDefault(o => o.Id == policy?.ClientId);

            return clientsByPolicy;
        }

        public async Task<ClientDto> GetClientByEmail(string email)
        {
            IEnumerable<ClientDto> clients = await GetClientsFromExternalService();
            ClientDto clientByEmail = clients.FirstOrDefault(o => o.Email.ToLower() == email.ToLower());

            return clientByEmail;
        }

        public async Task<IEnumerable<ClientDto>> GetClientsFromExternalService()
        {
            Client client = await this.clientProxy.GetEntityCollection(this.configuration["clientsServiceEndPoint"]);
            IEnumerable<ClientDto> clientsDto = client?.Clients;

            return clientsDto;
        }

        public async Task<IEnumerable<PolicyDto>> GetPoliciesFromExternalService()
        {
            Policy policy = await this.policyProxy.GetEntityCollection(this.configuration["policiesServiceEndPoint"]);
            IEnumerable<PolicyDto> policyDto = policy.Policies;

            return policyDto;
        }
    }
}