using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Insurance.Proxy.Contracts;
using Insurance.DTO.Model.Client;
using Insurance.DTO.Model.Policy;
using Insurance.Service.Contracts;

namespace Insurance.Service.Service
{
    public class ClientService : IClientService
    {
        public readonly IHttpProxy<Client> clientProxy;
        public readonly IHttpProxy<Policy> policyProxy;

        private const string clientsServiceEndPoint = "http://www.mocky.io/v2/5808862710000087232b75ac";
        private const string policiesServiceEndPoint = "http://www.mocky.io/v2/580891a4100000e8242b75c5";

        public ClientService(IHttpProxy<Client> httpClientProxy, IHttpProxy<Policy> policyProxy)
        {
            this.clientProxy = httpClientProxy;
            this.policyProxy = policyProxy;
        }

        public async Task<IEnumerable<ClientDto>> GetClients()
        {
            IEnumerable<ClientDto> clients = await GetClientsFromExternalService();

            return clients;
        }

        public async Task<ClientDto> GetClientById(Guid id)
        {
            IEnumerable<ClientDto> clients = await GetClientsFromExternalService();
            ClientDto clientsById = clients.Where(o => o.Id == id).FirstOrDefault();

            return clientsById;
        }

        public async Task<IEnumerable<ClientDto>> GetClientByName(string name)
        {
            IEnumerable<ClientDto> clients = await GetClientsFromExternalService();
            IEnumerable<ClientDto> clientsByName = clients.Where(o => o.Name == name);

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
            ClientDto clientByEmail = clients.Where(o => o.Email == email).FirstOrDefault();

            return clientByEmail;
        }

        private async Task<IEnumerable<ClientDto>> GetClientsFromExternalService()
        {
            Client client = await this.clientProxy.GetEntityCollection(clientsServiceEndPoint);
            IEnumerable<ClientDto> clientsDto = client.Clients;

            return clientsDto;
        }

        private async Task<IEnumerable<PolicyDto>> GetPoliciesFromExternalService()
        {
            Policy policy = await this.policyProxy.GetEntityCollection(policiesServiceEndPoint);
            IEnumerable<PolicyDto> policyDto = policy.Policies;

            return policyDto;
        }
    }
}