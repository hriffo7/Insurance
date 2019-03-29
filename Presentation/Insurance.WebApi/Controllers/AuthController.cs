using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.DTO.Model.Client;
using Insurance.Security.Contracts;
using Insurance.Service.Contracts;
using Insurance.WebApi.Filters;
using Insurance.WebApi.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        public IClientService clientService { get; set; }
        public IAuthentication authentication { get; set; }

        public AuthController(IClientService clientService, IAuthentication authentication)
        {
            this.clientService = clientService;
            this.authentication = authentication;
        }

        [HttpPost]
        [ServiceFilter(typeof(WebExceptionFilter))]
        [ServiceFilter(typeof(WebLoggerFilter))]
        public async Task<IActionResult> RequestToken([FromForm]AuthViewModel request)
        {
            ClientDto clientByEmail = await clientService.GetClientByEmail(request.Email);

            if (clientByEmail != null)
            {
                string token = this.authentication.GenerateJwtToken(clientByEmail);

                return Ok(token);
            }

            return Unauthorized();
        }
    }
}
