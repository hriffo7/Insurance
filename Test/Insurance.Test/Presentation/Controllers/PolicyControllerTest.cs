using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.DTO.Model.Policy;
using Insurance.Service.Contracts;
using Insurance.Service.Service;
using Insurance.WebApi;
using Insurance.WebApi.Controllers;
using Insurance.WebApi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System;
using AutoMapper;

namespace Insurance.Test.Presentation.Controllers
{
    [TestClass]
    public class PolicyControllerTest
    {
        private PolicyController policyController;

        private Mock<IPolicyService> mockerPolicyService;

        #region Constants

        private const int StatusCodeOK = 200;

        private const string ClientName = "Britney";

        private static readonly List<PolicyDto> policyDtoX0 = new List<PolicyDto>();

        private static readonly List<PolicyDto> policyDtoX1 = new List<PolicyDto>
        {
            new PolicyDto{
               Id = Guid.Parse("7b624ed3-00d5-4c1b-9ab8-c265067ef58b"),
                AmountInsured = 399.89M,
                Email = "inesblankenship@quotezart.com",
                InceptionDate = Convert.ToDateTime("2015-07-06T06:55:49Z"),
                InstallmentPayment = true,
                ClientId = Guid.Parse("a0ece5db-cd14-4f21-812f-966633e7be86")
        }};

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

        #endregion

        [TestInitialize]
        public void SetUp()
        {
            Startup.RegisterMaps();
            mockerPolicyService = new Mock<IPolicyService>();
            policyController = new PolicyController(mockerPolicyService.Object);

        }

        #region Constructor

        [TestMethod]
        public void PolicyController_WithParameters_VerifiesConstructorsObjects()
        {
            Assert.IsNotNull(policyController.policyService);
        }

        #endregion

        [TestCleanup]
        public void CleanUp()
        {
            Mapper.Reset();
        }

        #region GetPoliciesByClientName Action

        [TestMethod]
        public void GetByClientName_WithClientNameAsParameter_VerifyMethodFromLowerLayerThatReturnsTheListOfAssociatedPolicies()
        {
            mockerPolicyService.Setup(o => o.GetPoliciesByClientName(ClientName)).ReturnsAsync(policyDtoX0);

            OkObjectResult result = policyController.GetByClientName(ClientName).Result as OkObjectResult;

            mockerPolicyService.Verify(o => o.GetPoliciesByClientName(ClientName), Times.Once);
        }

        [TestMethod]
        public void GetByClientName_WithClientNameAsParameter_ReturnsAnEmptyListOfPoliciesWithStatusCodeOK()
        {
            mockerPolicyService.Setup(o => o.GetPoliciesByClientName(ClientName)).ReturnsAsync(policyDtoX0);

            OkObjectResult result = policyController.GetByClientName(ClientName).Result as OkObjectResult;
            List<PolicyViewModel> policyList = result.Value as List<PolicyViewModel>;

            Assert.AreEqual(StatusCodeOK, result.StatusCode);
            Assert.AreEqual(policyDtoX0.Count, policyList.Count());
        }

        [TestMethod]
        public void GetByClientName_WithClientNameAsParameter_ReturnsListOfPoliciesWithOneElementAndStatusCodeOK()
        {
            mockerPolicyService.Setup(o => o.GetPoliciesByClientName(ClientName)).ReturnsAsync(policyDtoX1);

            OkObjectResult result = policyController.GetByClientName(ClientName).Result as OkObjectResult;
            List<PolicyViewModel> policyList = result.Value as List<PolicyViewModel>;

            Assert.AreEqual(StatusCodeOK, result.StatusCode);
            Assert.AreEqual(policyDtoX1.Count, policyList.Count());
            Assert.AreEqual(policyDtoX1[0].Email, policyList[0].Email);
            Assert.AreEqual(policyDtoX1[0].ClientId, policyList[0].ClientId);
            Assert.AreEqual(policyDtoX1[0].AmountInsured, policyList[0].AmountInsured);
            Assert.AreEqual(policyDtoX1[0].InceptionDate, policyList[0].InceptionDate);
            Assert.AreEqual(policyDtoX1[0].InstallmentPayment, policyList[0].InstallmentPayment);
        }

        [TestMethod]
        public void GetByClientName_WithClientNameAsParameter_ReturnsListOfPoliciesWithTwoElementsAndStatusCodeOK()
        {
            mockerPolicyService.Setup(o => o.GetPoliciesByClientName(ClientName)).ReturnsAsync(policyDtoX2);

            OkObjectResult result = policyController.GetByClientName(ClientName).Result as OkObjectResult;
            List<PolicyViewModel> policyList = result.Value as List<PolicyViewModel>;

            Assert.AreEqual(StatusCodeOK, result.StatusCode);
            Assert.AreEqual(policyDtoX2.Count, policyList.Count());

            Assert.AreEqual(policyDtoX2[0].Email, policyList[0].Email);
            Assert.AreEqual(policyDtoX2[0].ClientId, policyList[0].ClientId);
            Assert.AreEqual(policyDtoX2[0].AmountInsured, policyList[0].AmountInsured);
            Assert.AreEqual(policyDtoX2[0].InceptionDate, policyList[0].InceptionDate);
            Assert.AreEqual(policyDtoX2[0].InstallmentPayment, policyList[0].InstallmentPayment);

            Assert.AreEqual(policyDtoX2[1].Email, policyList[1].Email);
            Assert.AreEqual(policyDtoX2[1].ClientId, policyList[1].ClientId);
            Assert.AreEqual(policyDtoX2[1].AmountInsured, policyList[1].AmountInsured);
            Assert.AreEqual(policyDtoX2[1].InceptionDate, policyList[1].InceptionDate);
            Assert.AreEqual(policyDtoX2[1].InstallmentPayment, policyList[1].InstallmentPayment);
        }

        #endregion
    }
}