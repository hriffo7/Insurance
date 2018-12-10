using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Service.Contracts;
using Insurance.Service.Service;
using Insurance.WebApi;
using Insurance.WebApi.Controllers;
using Insurance.WebApi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Insurance.Test.Presentation.Controllers
{
    [TestClass]
    public class PolicyControllerTest
    {
        private PolicyController policyController;
        private Mock<IPolicyService> policyService;


        [TestInitialize]
        public void SetUp()
        {
            Startup.RegisterMaps();
            policyService = new Mock<IPolicyService>();
            policyController = new PolicyController(policyService.Object);
        }

        [TestMethod]
        public void PolicyController_GET_GetByClientName_VerifyOkResponse()
        {
            OkObjectResult result = policyController.GetByClientName("Britney").Result as OkObjectResult;

            //var test = policyService.Setup(o => o.GetPoliciesByClientName("pepe")).Returns();

            Assert.AreEqual(200, result.StatusCode);
        }
    }
}