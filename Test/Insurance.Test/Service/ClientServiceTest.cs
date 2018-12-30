using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.DTO.Model.Client;
using Insurance.DTO.Model.Policy;
using Insurance.Proxy.Contracts;
using Insurance.Service.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Insurance.Test.Service
{
    [TestClass]
    public class ClientServiceTest
    {
        private ClientService clientService;
        private Mock<IHttpProxy<Client>> mockerClientProxy;
        private Mock<IHttpProxy<Policy>> mockerPolicyProxy;
        private Mock<IConfiguration> mockerConfiguration;

        #region Constants

        private static readonly List<ClientDto> clientDtoX2 = new List<ClientDto>{
             new ClientDto{
                Id = Guid.Parse("a0ece5db-cd14-4f21-812f-966633e7be86"),
                Name = "Britney",
                Email = "britneyblankenship@quotezart.com",
                Role = "admin"
             },
                 new ClientDto{
                Id = Guid.Parse("a0ece5db-cd14-4f21-812f-966633e7be78"),
                Name = "Britney",
                Email = "britney@quotezart.com",
                Role = "user"
             },
             new ClientDto{
                Id = Guid.Parse("a3b8d425-2b60-4ad7-becc-bedf2ef860bd"),
                Name = "Barnett",
                Email = "barnettblankenship@quotezart.com",
                Role = "user"
             }
         };

        private static readonly Client clientX2 = new Client()
        {
            Clients = clientDtoX2
        };

        private static readonly List<PolicyDto> policyDtoX2 = new List<PolicyDto>
        {
            new PolicyDto
            {
               Id = Guid.Parse("7b624ed3-00d5-4c1b-9ab8-c265067ef58b"),
                AmountInsured = 399.89M,
                Email = "inesblankenship@quotezart.com",
                InceptionDate = Convert.ToDateTime("2015-07-06T06:55:49Z"),
                InstallmentPayment = true,
                ClientId = Guid.Parse("a0ece5db-cd14-4f21-812f-966633e7be86")
        },
            new PolicyDto
            {
                Id = Guid.Parse("6f514ec4-1726-4628-974d-20afe4da130c"),
                AmountInsured = 697.04M,
                Email = "inesblankenship@quotezart.com",
                InceptionDate = Convert.ToDateTime("2014-09-12T12:10:23Z"),
                InstallmentPayment = false,
                ClientId = Guid.Parse("a0ece5db-cd14-4f21-812f-966633e7be86")
            }
        };

        private static readonly Policy policyX2 = new Policy()
        {
            Policies = policyDtoX2
        };

        private Guid ClientId = Guid.Parse("a0ece5db-cd14-4f21-812f-966633e7be86");

        private Guid PolicyId = Guid.Parse("7b624ed3-00d5-4c1b-9ab8-c265067ef58b");

        private Guid FakePolicyId = Guid.Parse("64cceef9-3a01-49ae-a23b-3761b604800b");

        private string ClientName = "Barnett";

        private string DuplicatedClientName = "Britney";

        private string NonExistentClientName = "Oscar";

        private string ClientEmail = "britneyblankenship@quotezart.com";

        #endregion

        [TestInitialize]
        public void SetUp()
        {
            mockerClientProxy = new Mock<IHttpProxy<Client>>();
            mockerPolicyProxy = new Mock<IHttpProxy<Policy>>();
            mockerConfiguration = new Mock<IConfiguration>();

            clientService = new ClientService(mockerClientProxy.Object, mockerPolicyProxy.Object, mockerConfiguration.Object);
        }

        #region Constructor

        [TestMethod]
        public void ClientService_WithParameters_VerifiesConstructorsObjects()
        {
            Assert.IsNotNull(clientService.clientProxy);
            Assert.IsNotNull(clientService.policyProxy);
            Assert.IsNotNull(clientService.configuration);
        }

        #endregion

        #region GetClientsFromExternalService

        [TestMethod]
        public void GetClientsFromExternalService_WithApiEndpointAsParameter_ReturnsTheCollectionOfEntityFromMockedExternalService()
        {
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            clientService.GetClientsFromExternalService().Wait();

            mockerClientProxy.Verify(o => o.GetEntityCollection(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetClientsFromExternalService_WithApiEndpointAsParameter_VerifiesMethodFromLowerLayerThatReturnsTheCollectionOfEntity()
        {
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = clientService.GetClientsFromExternalService().Result;
            IEnumerable<ClientDto> clientDtoList = result as IEnumerable<ClientDto>;

            Assert.AreEqual(result.Count(), clientX2.Clients.Count());
        }

        #endregion

        #region GetPoliciesFromExternalService

        [TestMethod]
        public void GetPoliciesFromExternalService_WithApiEndpointAsParameter_ReturnsTheCollectionOfEntityFromMockedExternalService()
        {
            mockerPolicyProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(policyX2);

            clientService.GetPoliciesFromExternalService().Wait();

            mockerPolicyProxy.Verify(o => o.GetEntityCollection(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetPoliciesFromExternalService_WithApiEndpointAsParameter_VerifiesMethodFromLowerLayerThatReturnsTheCollectionOfEntity()
        {
            mockerPolicyProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(policyX2);

            var result = clientService.GetPoliciesFromExternalService().Result;
            IEnumerable<PolicyDto> clientDtoList = result as IEnumerable<PolicyDto>;

            Assert.AreEqual(result.Count(), policyX2.Policies.Count());
        }

        #endregion

        #region GetClientById

        [TestMethod]
        public void GetClientById_WithIdAsParameter_VerifiesMethodThatReturnsTheClientById()
        {
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = this.clientService.GetClientById(ClientId);

            mockerClientProxy.Verify(o => o.GetEntityCollection(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetClientById_WithIdAsParameter_ReturnsTheClientById()
        {
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = this.clientService.GetClientById(ClientId);

            Assert.AreEqual(result.Result.Id, clientDtoX2[0].Id);
            Assert.AreEqual(result.Result.Email, clientDtoX2[0].Email);
            Assert.AreEqual(result.Result.Name, clientDtoX2[0].Name);
            Assert.AreEqual(result.Result.Role, clientDtoX2[0].Role);
        }

        #endregion


        #region GetClientByName

        [TestMethod]
        public void GetClientByName_WithNameAsParameter_VerifiesMethodThatReturnsTheClientByName()
        {
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = this.clientService.GetClientByName(ClientName);

            mockerClientProxy.Verify(o => o.GetEntityCollection(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetClientByName_WithNameAsParameter_ReturnsTheClientByNameWithouthMatches()
        {
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = this.clientService.GetClientByName(NonExistentClientName).Result.ToList();

            List<ClientDto> clientDtoList = result as List<ClientDto>;

            Assert.AreEqual(clientDtoList.Count, 0);
        }

        [TestMethod]
        public void GetClientByName_WithNameAsParameter_ReturnsTheClientByNameWithOneMatch()
        {
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = this.clientService.GetClientByName(ClientName).Result.ToList();

            List<ClientDto> clientDtoList = result as List<ClientDto>;

            Assert.AreEqual(clientDtoList[0].Id, clientDtoX2[2].Id);
            Assert.AreEqual(clientDtoList[0].Email, clientDtoX2[2].Email);
            Assert.AreEqual(clientDtoList[0].Name, clientDtoX2[2].Name);
            Assert.AreEqual(clientDtoList[0].Role, clientDtoX2[2].Role);
        }

        [TestMethod]
        public void GetClientByName_WithNameAsParameter_ReturnsTheClientByNameWithTwoMatches()
        {
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = this.clientService.GetClientByName(DuplicatedClientName).Result.ToList();

            List<ClientDto> clientDtoList = result as List<ClientDto>;

            Assert.AreEqual(clientDtoList[0].Id, clientDtoX2[0].Id);
            Assert.AreEqual(clientDtoList[0].Email, clientDtoX2[0].Email);
            Assert.AreEqual(clientDtoList[0].Name, clientDtoX2[0].Name);
            Assert.AreEqual(clientDtoList[0].Role, clientDtoX2[0].Role);

            Assert.AreEqual(clientDtoList[1].Id, clientDtoX2[1].Id);
            Assert.AreEqual(clientDtoList[1].Email, clientDtoX2[1].Email);
            Assert.AreEqual(clientDtoList[1].Name, clientDtoX2[1].Name);
            Assert.AreEqual(clientDtoList[1].Role, clientDtoX2[1].Role);
        }

        #endregion

        #region  GetClientByEmail

        [TestMethod]
        public void GetClientByEmail_WithEmailAsParameter_VerifiesMethodThatReturnsTheClientByEmail()
        {
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = this.clientService.GetClientByEmail(ClientEmail);

            mockerClientProxy.Verify(o => o.GetEntityCollection(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetClientByEmail_WithEmailAsParameter_ReturnsTheClientByEmail()
        {
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = this.clientService.GetClientByEmail(ClientEmail);

            Assert.AreEqual(result.Result.Id, clientDtoX2[0].Id);
            Assert.AreEqual(result.Result.Email, clientDtoX2[0].Email);
            Assert.AreEqual(result.Result.Name, clientDtoX2[0].Name);
            Assert.AreEqual(result.Result.Role, clientDtoX2[0].Role);
        }

        #endregion

        #region  GetClientByPolicyId

        [TestMethod]
        public void GetClientByPolicyId_WithPolicyIdAsParameter_VerifiesMethodThatReturnsTheClientByPolicyId()
        {
            mockerPolicyProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(policyX2);
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = this.clientService.GetClientByPolicyId(PolicyId);

            mockerClientProxy.Verify(o => o.GetEntityCollection(It.IsAny<string>()), Times.Once);
            mockerPolicyProxy.Verify(o => o.GetEntityCollection(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetClientByPolicyId_WithPolicyIdAsParameter_ReturnsTheClientByPolicyId()
        {
            mockerPolicyProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(policyX2);
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = this.clientService.GetClientByPolicyId(PolicyId);

            Assert.AreEqual(result.Result.Id, clientDtoX2[0].Id);
            Assert.AreEqual(result.Result.Email, clientDtoX2[0].Email);
            Assert.AreEqual(result.Result.Name, clientDtoX2[0].Name);
            Assert.AreEqual(result.Result.Role, clientDtoX2[0].Role);
        }

        [TestMethod]
        public void GetClientByPolicyId_WithFakePolicyIdAsParameter_ReturnsTheClientByPolicyId()
        {
            mockerPolicyProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(policyX2);
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = this.clientService.GetClientByPolicyId(FakePolicyId);

            Assert.IsNull(result.Result);
        }

        #endregion
    }
}