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
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        public IClientService clientService { get; set; }

        public ClientController(IClientService userService)
        {
            this.clientService = userService;
        }

        [HttpGet]
        [Route("GetById/{clientId}")]
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
        [Route("GetByName/{name}")]
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
        [Route("GetByPolicyId/{policyId}")]
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
