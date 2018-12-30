using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PolicyServiceTest
    {
        private PolicyService policyService;
        private Mock<IHttpProxy<Client>> mockerClientProxy;
        private Mock<IHttpProxy<Policy>> mockerPolicyProxy;
        private Mock<IConfiguration> mockerConfiguration;

        #region Constants

        private string ClientName = "Britney";

        private string NonExistentClientName = "Oscar";

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

        private static readonly List<PolicyDto> policyDtoX1 = new List<PolicyDto>
        {
            new PolicyDto
            {
               Id = Guid.Parse("7b624ed3-00d5-4c1b-9ab8-c265067ef58b"),
                AmountInsured = 399.89M,
                Email = "inesblankenship@quotezart.com",
                InceptionDate = Convert.ToDateTime("2015-07-06T06:55:49Z"),
                InstallmentPayment = true,
                ClientId = Guid.Parse("a0ece5db-cd14-4f21-812f-966633e7be86")
        }
        };

        private static readonly Policy policyX1 = new Policy()
        {
            Policies = policyDtoX1
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

        #endregion

        [TestInitialize]
        public void SetUp()
        {
            mockerClientProxy = new Mock<IHttpProxy<Client>>();
            mockerPolicyProxy = new Mock<IHttpProxy<Policy>>();
            mockerConfiguration = new Mock<IConfiguration>();

            policyService = new PolicyService(mockerPolicyProxy.Object, mockerClientProxy.Object, mockerConfiguration.Object);
        }

        #region Constructor

        [TestMethod]
        public void PolicyService_WithParameters_VerifiesConstructorsObjects()
        {
            Assert.IsNotNull(policyService.clientProxy);
            Assert.IsNotNull(policyService.policyProxy);
            Assert.IsNotNull(policyService.configuration);
        }

        #endregion

        #region GetClientsFromExternalService

        [TestMethod]
        public void GetClientsFromExternalService_WithApiEndpointAsParameter_ReturnsTheCollectionOfEntityFromMockedExternalService()
        {
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            policyService.GetClientsFromExternalService().Wait();

            mockerClientProxy.Verify(o => o.GetEntityCollection(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetClientsFromExternalService_WithApiEndpointAsParameter_VerifiesMethodFromLowerLayerThatReturnsTheCollectionOfEntity()
        {
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = policyService.GetClientsFromExternalService().Result;
            IEnumerable<ClientDto> clientDtoList = result as IEnumerable<ClientDto>;

            Assert.AreEqual(result.Count(), clientX2.Clients.Count());
        }

        #endregion

        #region GetPoliciesFromExternalService

        [TestMethod]
        public void GetPoliciesFromExternalService_WithApiEndpointAsParameter_ReturnsTheCollectionOfEntityFromMockedExternalService()
        {
            mockerPolicyProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(policyX2);

            policyService.GetPoliciesFromExternalService().Wait();

            mockerPolicyProxy.Verify(o => o.GetEntityCollection(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetPoliciesFromExternalService_WithApiEndpointAsParameter_VerifiesMethodFromLowerLayerThatReturnsTheCollectionOfEntity()
        {
            mockerPolicyProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(policyX2);

            var result = policyService.GetPoliciesFromExternalService().Result;
            IEnumerable<PolicyDto> clientDtoList = result as IEnumerable<PolicyDto>;

            Assert.AreEqual(result.Count(), policyX2.Policies.Count());
        }

        #endregion

        #region GetPoliciesByClientName

        [TestMethod]
        public void GetPoliciesByClientName_WithClientNameAsParameter_VerifiesMethodThatReturnsTheListOfPoliciesByClientName()
        {
            mockerPolicyProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(policyX2);
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = this.policyService.GetPoliciesByClientName(ClientName);

            mockerClientProxy.Verify(o => o.GetEntityCollection(It.IsAny<string>()), Times.Once);
            mockerPolicyProxy.Verify(o => o.GetEntityCollection(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetPoliciesByClientName_WithNonExistentClientNameAsParameter_ReturnsTheEmptyListOfPolicies()
        {
            mockerPolicyProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(policyX2);
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = this.policyService.GetPoliciesByClientName(NonExistentClientName);

            Assert.AreEqual(result.Result.Count(), 0);
        }

        [TestMethod]
        public void GetPoliciesByClientName_WithClientNameIdAsParameter_ReturnsTheListOfPoliciesByClientNameX1()
        {
            mockerPolicyProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(policyX1);
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = this.policyService.GetPoliciesByClientName(ClientName).Result;
            List<PolicyDto> policyDtoList = result as List<PolicyDto>;

            List<PolicyDto> policyList = policyX1.Policies.ToList();

            Assert.AreEqual(policyDtoList[0].Id, policyList[0].Id);
            Assert.AreEqual(policyDtoList[0].AmountInsured, policyList[0].AmountInsured);
            Assert.AreEqual(policyDtoList[0].ClientId, policyList[0].ClientId);
            Assert.AreEqual(policyDtoList[0].Email, policyList[0].Email);
            Assert.AreEqual(policyDtoList[0].InceptionDate, policyList[0].InceptionDate);
            Assert.AreEqual(policyDtoList[0].InstallmentPayment, policyList[0].InstallmentPayment);
        }

        [TestMethod]
        public void GetPoliciesByClientName_WithClientNameIdAsParameter_ReturnsTheListOfPoliciesByClientNameX2()
        {
            mockerPolicyProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(policyX2);
            mockerClientProxy.Setup(o => o.GetEntityCollection(It.IsAny<string>())).ReturnsAsync(clientX2);

            var result = this.policyService.GetPoliciesByClientName(ClientName).Result;
            List<PolicyDto> policyDtoList = result as List<PolicyDto>;

            List<PolicyDto> policyList = policyX2.Policies.ToList();

            Assert.AreEqual(policyDtoList[0].Id, policyList[0].Id);
            Assert.AreEqual(policyDtoList[0].AmountInsured, policyList[0].AmountInsured);
            Assert.AreEqual(policyDtoList[0].ClientId, policyList[0].ClientId);
            Assert.AreEqual(policyDtoList[0].Email, policyList[0].Email);
            Assert.AreEqual(policyDtoList[0].InceptionDate, policyList[0].InceptionDate);
            Assert.AreEqual(policyDtoList[0].InstallmentPayment, policyList[0].InstallmentPayment);

            Assert.AreEqual(policyDtoList[1].Id, policyList[1].Id);
            Assert.AreEqual(policyDtoList[1].AmountInsured, policyList[1].AmountInsured);
            Assert.AreEqual(policyDtoList[1].ClientId, policyList[1].ClientId);
            Assert.AreEqual(policyDtoList[1].Email, policyList[1].Email);
            Assert.AreEqual(policyDtoList[1].InceptionDate, policyList[1].InceptionDate);
            Assert.AreEqual(policyDtoList[1].InstallmentPayment, policyList[1].InstallmentPayment);
        }

        #endregion
    }
}