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
        [Route("GetUsers")]
        //[Authorize(Roles = "user, admin")]
        [ServiceFilter(typeof(WebExceptionFilter))]
        [ServiceFilter(typeof(WebLoggerFilter))]
        public async Task<IActionResult> GetUsers()
        {
            IEnumerable<ClientDto> clientDto = await clientService.GetClients();
            IEnumerable<ClientViewModel> clientViewModel = AutoMapper.Mapper.Map<IEnumerable<ClientViewModel>>(clientDto);

            return Ok(clientViewModel);
        }

        [HttpGet]
        [Route("GetById/{clientId}")]
        [Authorize(Roles = "user, admin")]
        [ServiceFilter(typeof(WebExceptionFilter))]
        [ServiceFilter(typeof(WebLoggerFilter))]
        public async Task<IActionResult> GetById(Guid clientId)
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
        public async Task<IActionResult> GetByName(string name)
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
        public async Task<IActionResult> GetByPolicyId(Guid policyId)
        {
            ClientDto clientDto = await clientService.GetClientByPolicyId(policyId);
            ClientViewModel clientViewModel = AutoMapper.Mapper.Map<ClientViewModel>(clientDto);

            return Ok(clientViewModel);
        }
    }
}
