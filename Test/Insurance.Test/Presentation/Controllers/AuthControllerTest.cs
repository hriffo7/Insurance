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
using Insurance.Security.Contracts;

namespace Insurance.Test.Presentation.Controllers
{
    [TestClass]
    public class AuthControllerTest
    {
        private AuthController authController;
        private Mock<IAuthentication> mockerAuthService;
        private Mock<IClientService> mockerClientService;

        private const int StatusCodeOK = 200;

        private const int StatusCodeUnauthorized = 401;

        private const string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJCcml0bmV5IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiYWRtaW4iLCJpZCI6ImEwZWNlNWRiLWNkMTQtNGYyMS04MTJmLTk2NjYzM2U3YmU4NiIsImV4cCI6MTU0NTQwMDI4NX0.zRITkeMHsapNXfY2Dt5kECAJBNKXatkqseR6TAlfsQA";

        private const string invalidEmail = "britneyblankenship@invalid.com";

        private static readonly AuthViewModel authViewModel = new AuthViewModel()
        {
            Email = "britneyblankenship@quotezart.com"
        };

        private static readonly ClientDto emptyClientDto = new ClientDto() { };

        private static readonly ClientDto clientDto = new ClientDto()
        {
            Id = Guid.Parse("a0ece5db-cd14-4f21-812f-966633e7be86"),
            Name = "Britney",
            Email = "britneyblankenship@quotezart.com",
            Role = "admin"
        };

        [TestInitialize]
        public void SetUp()
        {
            // Startup.RegisterMaps();
            mockerAuthService = new Mock<IAuthentication>();
            mockerClientService = new Mock<IClientService>();
            authController = new AuthController(mockerClientService.Object, mockerAuthService.Object);
        }

        #region RequestToken Action

        [TestMethod]
        public void RequestToken_WithValidUserEmail_VerifyMethodFromLowerLayerThatGeneratesJWTWithStatusCodeOK()
        {
            mockerClientService.Setup(o => o.GetClientByEmail(authViewModel.Email)).ReturnsAsync(clientDto);
            mockerAuthService.Setup(o => o.GenerateJwtToken(clientDto)).Returns(token);

            OkObjectResult result = authController.RequestToken(authViewModel).Result as OkObjectResult;

            mockerAuthService.Verify(o => o.GenerateJwtToken(clientDto), Times.Once);
            Assert.AreEqual(StatusCodeOK, result.StatusCode);
        }

        [TestMethod]
        public void RequestToken_WithInvalidUserEmail_VerifiesTheMethodThatGeneratesTokenIsNeverCalledAndReturnsUnauthorizedStatusCode()
        {
            mockerClientService.Setup(o => o.GetClientByEmail(invalidEmail)).ReturnsAsync(emptyClientDto);

            UnauthorizedResult result = authController.RequestToken(authViewModel).Result as UnauthorizedResult;

            mockerAuthService.Verify(o => o.GenerateJwtToken(clientDto), Times.Never);
            Assert.AreEqual(StatusCodeUnauthorized, result.StatusCode);
        }

        #endregion
    }
}