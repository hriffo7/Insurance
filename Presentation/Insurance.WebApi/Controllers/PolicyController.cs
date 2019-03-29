using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.DTO.Model.Policy;
using Insurance.Service.Contracts;
using Insurance.WebApi.Filters;
using Insurance.WebApi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class PolicyController : Controller
    {
        public IPolicyService policyService { get; set; }

        public PolicyController(IPolicyService policyService)
        {
            this.policyService = policyService;
        }

        [HttpGet]
        [Route("{clientName}")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(WebExceptionFilter))]
        [ServiceFilter(typeof(WebLoggerFilter))]
        public async Task<IActionResult> GetByClientName(string clientName)
        {
            IEnumerable<PolicyDto> policiesDto = await policyService.GetPoliciesByClientName(clientName);
            IEnumerable<PolicyViewModel> policiesViewModel = AutoMapper.Mapper.Map<IEnumerable<PolicyViewModel>>(policiesDto);

            return Ok(policiesViewModel);
        }
    }
}
