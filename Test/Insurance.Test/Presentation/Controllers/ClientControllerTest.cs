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
using Insurance.DTO.Model.Client;

namespace Insurance.Test.Presentation.Controllers
{
    [TestClass]
    public class ClientControllerTest
    {
        private ClientController clientController;

        private Mock<IClientService> mockerClientService;

        #region Constants

        private const int StatusCodeOK = 200;

        private const string ClientName = "Britney";

        private Guid ClientId = Guid.Parse("a0ece5db-cd14-4f21-812f-966633e7be86");

        private Guid PolicyId = Guid.Parse("7b624ed3-00d5-4c1b-9ab8-c265067ef58b");

        private static readonly List<ClientDto> clientDtoX0 = new List<ClientDto>();

        private static readonly List<ClientDto> clientDtoX1 = new List<ClientDto>{
             new ClientDto{
                Id = Guid.Parse("a0ece5db-cd14-4f21-812f-966633e7be86"),
                Name = "Britney",
                Email = "britneyblankenship@quotezart.com",
                Role = "admin"
             }
         };

        private static readonly List<ClientDto> clientDtoX2 = new List<ClientDto>{
             new ClientDto{
                Id = Guid.Parse("a0ece5db-cd14-4f21-812f-966633e7be86"),
                Name = "Britney",
                Email = "britneyblankenship@quotezart.com",
                Role = "admin"
             },
             new ClientDto{
                Id = Guid.Parse("a3b8d425-2b60-4ad7-becc-bedf2ef860bd"),
                Name = "Barnett",
                Email = "barnettblankenship@quotezart.com",
                Role = "user"
             }
         };

        private static readonly ClientDto clientDto = new ClientDto()
        {
            Id = Guid.Parse("a0ece5db-cd14-4f21-812f-966633e7be86"),
            Name = "Britney",
            Email = "britneyblankenship@quotezart.com",
            Role = "admin"
        };

        #endregion

        [TestInitialize]
        public void SetUp()
        {
            Startup.RegisterMaps();
            mockerClientService = new Mock<IClientService>();
            clientController = new ClientController(mockerClientService.Object);

        }

        [TestCleanup]
        public void CleanUp()
        {
            Mapper.Reset();
        }

        #region Constructor

        [TestMethod]
        public void ClientController_WithParameters_VerifiesConstructorsObjects()
        {
            Assert.IsNotNull(clientController.clientService);
        }

        #endregion

        #region GetClientById Action

        [TestMethod]
        public void GetClientById_WithIdAsParameter_VerifyMethodFromLowerLayerThatReturnsTheAssociatedClient()
        {
            mockerClientService.Setup(o => o.GetClientById(ClientId)).ReturnsAsync(clientDto);

            OkObjectResult result = clientController.GetClientById(ClientId).Result as OkObjectResult;

            mockerClientService.Verify(o => o.GetClientById(ClientId), Times.Once);
        }

        [TestMethod]
        public void GetClientById_WithIdAsParameter_ReturnsOneClientWithStatusCodeOK()
        {
            mockerClientService.Setup(o => o.GetClientById(ClientId)).ReturnsAsync(clientDto);

            OkObjectResult result = clientController.GetClientById(ClientId).Result as OkObjectResult;
            ClientViewModel clientViewModel = result.Value as ClientViewModel;

            Assert.AreEqual(StatusCodeOK, result.StatusCode);
            Assert.AreEqual(clientDto.Id, clientViewModel.Id);
            Assert.AreEqual(clientDto.Name, clientViewModel.Name);
            Assert.AreEqual(clientDto.Email, clientViewModel.Email);
            Assert.AreEqual(clientDto.Role, clientViewModel.Role);
        }

        #endregion


        #region GetClientByPolicyId Action

        [TestMethod]
        public void GetClientByPolicyId_WithPolicyIdAsParameter_VerifyMethodFromLowerLayerThatReturnsTheAssociatedClient()
        {
            mockerClientService.Setup(o => o.GetClientByPolicyId(PolicyId)).ReturnsAsync(clientDto);

            OkObjectResult result = clientController.GetClientByPolicyId(PolicyId).Result as OkObjectResult;

            mockerClientService.Verify(o => o.GetClientByPolicyId(PolicyId), Times.Once);
        }

        [TestMethod]
        public void GetClientByPolicyId_WithPolicyIdAsParameter_ReturnsOneClientWithStatusCodeOK()
        {
            mockerClientService.Setup(o => o.GetClientByPolicyId(PolicyId)).ReturnsAsync(clientDto);

            OkObjectResult result = clientController.GetClientByPolicyId(PolicyId).Result as OkObjectResult;
            ClientViewModel clientViewModel = result.Value as ClientViewModel;

            Assert.AreEqual(StatusCodeOK, result.StatusCode);
            Assert.AreEqual(clientDto.Id, clientViewModel.Id);
            Assert.AreEqual(clientDto.Name, clientViewModel.Name);
            Assert.AreEqual(clientDto.Email, clientViewModel.Email);
            Assert.AreEqual(clientDto.Role, clientViewModel.Role);
        }

        #endregion


        #region GetClientByName Action

        [TestMethod]
        public void GetClientByName_WithClientNameAsParameter_VerifyMethodFromLowerLayerThatReturnsTheListOfClientsWithSpecificName()
        {
            mockerClientService.Setup(o => o.GetClientByName(ClientName)).ReturnsAsync(clientDtoX0);

            OkObjectResult result = clientController.GetClientByName(ClientName).Result as OkObjectResult;

            mockerClientService.Verify(o => o.GetClientByName(ClientName), Times.Once);
        }

        [TestMethod]
        public void GetClientByName_WithClientNameAsParameter_ReturnsAnEmptyListOfClientsWithStatusCodeOK()
        {
            mockerClientService.Setup(o => o.GetClientByName(ClientName)).ReturnsAsync(clientDtoX0);

            OkObjectResult result = clientController.GetClientByName(ClientName).Result as OkObjectResult;
            List<ClientViewModel> clientList = result.Value as List<ClientViewModel>;

            Assert.AreEqual(StatusCodeOK, result.StatusCode);
            Assert.AreEqual(clientDtoX0.Count, clientList.Count());
        }

        [TestMethod]
        public void GetClientByName_WithClientNameAsParameter_ReturnsListOfClientsWithOneElementAndStatusCodeOK()
        {
            mockerClientService.Setup(o => o.GetClientByName(ClientName)).ReturnsAsync(clientDtoX1);

            OkObjectResult result = clientController.GetClientByName(ClientName).Result as OkObjectResult;
            List<ClientViewModel> clientList = result.Value as List<ClientViewModel>;

            Assert.AreEqual(StatusCodeOK, result.StatusCode);
            Assert.AreEqual(clientDtoX1.Count, clientList.Count());
            Assert.AreEqual(clientDtoX1[0].Id, clientList[0].Id);
            Assert.AreEqual(clientDtoX1[0].Email, clientList[0].Email);
            Assert.AreEqual(clientDtoX1[0].Name, clientList[0].Name);
            Assert.AreEqual(clientDtoX1[0].Role, clientList[0].Role);
        }

        [TestMethod]
        public void GetClientByName_WithClientNameAsParameter_ReturnsListOfClientsWithTwoElementsAndStatusCodeOK()
        {
            mockerClientService.Setup(o => o.GetClientByName(ClientName)).ReturnsAsync(clientDtoX2);

            OkObjectResult result = clientController.GetClientByName(ClientName).Result as OkObjectResult;
            List<ClientViewModel> clientList = result.Value as List<ClientViewModel>;

            Assert.AreEqual(StatusCodeOK, result.StatusCode);
            Assert.AreEqual(clientDtoX2.Count, clientList.Count());
            Assert.AreEqual(clientDtoX2[0].Id, clientList[0].Id);
            Assert.AreEqual(clientDtoX2[0].Email, clientList[0].Email);
            Assert.AreEqual(clientDtoX2[0].Name, clientList[0].Name);
            Assert.AreEqual(clientDtoX2[0].Role, clientList[0].Role);

            Assert.AreEqual(clientDtoX2[1].Id, clientList[1].Id);
            Assert.AreEqual(clientDtoX2[1].Email, clientList[1].Email);
            Assert.AreEqual(clientDtoX2[1].Name, clientList[1].Name);
            Assert.AreEqual(clientDtoX2[1].Role, clientList[1].Role);
        }

        #endregion
    }
}