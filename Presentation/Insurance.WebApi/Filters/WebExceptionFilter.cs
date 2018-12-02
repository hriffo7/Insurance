using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
            Exception ex = context.Exception;

            logger.LogError(ex.Message);
            if (ex.InnerException != null)
                logger.LogError(ex.InnerException.ToString());
            logger.LogError(context.ActionDescriptor.DisplayName);
            logger.LogError(">>>>>>>>>>>>>>>>>>> END WEB APPLICATION ERROR <<<<<<<<<<<<<<<<<<<<<");
            context.ExceptionHandled = true;

            HttpStatusCode code = HttpStatusCode.InternalServerError; // 500 if unexpected

            // Evaluate different error types codes extending Exception class

            string result = JsonConvert.SerializeObject(new { error = ex.Message });
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;
            byte[] data = Encoding.UTF8.GetBytes(result);
            context.HttpContext.Response.Body.WriteAsync(data);
        }
    }
}
