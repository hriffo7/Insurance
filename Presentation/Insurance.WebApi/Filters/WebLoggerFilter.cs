using System;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Insurance.WebApi.Filters
{
    public class WebLoggerFilter : ActionFilterAttribute
    {
        private ILogger logger;

        public WebLoggerFilter(ILoggerFactory pfactory)
        {
            logger = pfactory.AddConsole().AddDebug().CreateLogger<WebLoggerFilter>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string action = context.RouteData.Values["Action"].ToString();
            string controller = context.RouteData.Values["Controller"].ToString();
            StringBuilder builder = new StringBuilder();
            builder.Append("/// >>>>>>>>>>>>").Append("Web Request: Performing request to service layer -- Controller: ")
                    .Append(controller).Append(" Action: ").Append(action).Append(@" <<<<<<<<<< \\\");
            logger.LogInformation(builder.ToString());
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            bool withErrors = context.Exception == null;
            string action = context.RouteData.Values["Action"].ToString();
            string controller = context.RouteData.Values["Controller"].ToString();
            StringBuilder builder = new StringBuilder();
            builder.Append("/// >>>>>>>>>>>>").Append("Web Request: Operations performed on Controller: ")
                    .Append(controller).Append(" Action: ").Append(action).Append((context.Exception != null) ? "Success Result" : "").Append(@"<<<<<<< \\\");
            logger.LogInformation(builder.ToString());
            logger.LogInformation(@"/// >>>>>>>>>>>>>>>>>>> APPLICATION LOG <<<<<<<<<<<<<<<<<<<< \\\");
            base.OnActionExecuted(context);
        }
    }
}
