using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.DTO.Model.Client;
using Insurance.Service.Contracts;
using Insurance.WebApi.Filters;
using Insurance.WebApi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers
{
    public class ClientController : Controller
    {
        public IClientService clientService { get; set; }

        public ClientController(IClientService userService)
        {
            this.clientService = userService;
        }

        [HttpGet]
        [Route("api/Client/{clientId}")]
        [Authorize(Roles = "user, admin")]
        [ServiceFilter(typeof(WebExceptionFilter))]
        [ServiceFilter(typeof(WebLoggerFilter))]
        public async Task<IActionResult> GetClientById(Guid clientId)
        {
            ClientDto clientDto = await clientService.GetClientById(clientId);
            ClientViewModel clientViewModel = AutoMapper.Mapper.Map<ClientViewModel>(clientDto);

            return Ok(clientViewModel);
        }

        [HttpGet]
        [Route("api/Client/Name/{name}")]
        [Authorize(Roles = "user, admin")]
        [ServiceFilter(typeof(WebExceptionFilter))]
        [ServiceFilter(typeof(WebLoggerFilter))]
        public async Task<IActionResult> GetClientByName(string name)
        {
            IEnumerable<ClientDto> clientDto = await clientService.GetClientByName(name);
            IEnumerable<ClientViewModel> clientViewModel = AutoMapper.Mapper.Map<IEnumerable<ClientViewModel>>(clientDto);

            return Ok(clientViewModel);
        }

        [HttpGet]
        [Route("api/Policies/{policyId}/Clients")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(WebExceptionFilter))]
        [ServiceFilter(typeof(WebLoggerFilter))]
        public async Task<IActionResult> GetClientByPolicyId(Guid policyId)
        {
            ClientDto clientDto = await clientService.GetClientByPolicyId(policyId);
            ClientViewModel clientViewModel = AutoMapper.Mapper.Map<ClientViewModel>(clientDto);

            return Ok(clientViewModel);
        }
    }
}
