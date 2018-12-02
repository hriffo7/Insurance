using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Insurance.WebApi.Filters
{
    public class WebExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger logger;

        public WebExceptionFilter(ILogger<WebExceptionFilter> plogger)
        {
            logger = plogger;
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(">>>>>>>>>>>>>>>>>>> WEB APPLICATION ERROR <<<<<<<<<<<<<<<<<<<<<<<");

        }
    }
}
