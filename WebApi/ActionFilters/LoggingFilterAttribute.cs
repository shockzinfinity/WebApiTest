using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;
using WebApi.Helpers;

namespace WebApi.ActionFilters
{
	public class LoggingFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new NLogger());
			var trace = GlobalConfiguration.Configuration.Services.GetTraceWriter();
			trace.Info(actionContext.Request, "Controller: " + actionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName + Environment.NewLine + "Action: " + actionContext.ActionDescriptor.ActionName, "JSON", actionContext.ActionArguments);
		}
	}
}