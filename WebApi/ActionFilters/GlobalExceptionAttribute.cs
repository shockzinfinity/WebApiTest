using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;
using WebApi.ErrorHelper;
using WebApi.Helpers;

namespace WebApi.ActionFilters
{
	public class GlobalExceptionAttribute : ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new NLogger());
			var trace = GlobalConfiguration.Configuration.Services.GetTraceWriter();
			trace.Error(actionExecutedContext.Request, "Controller: " + actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName + Environment.NewLine + "Action: " + actionExecutedContext.ActionContext.ActionDescriptor.ActionName, actionExecutedContext.Exception);

			var exceptionType = actionExecutedContext.Exception.GetType();
			if (exceptionType == typeof(ValidationException))
			{
				var response = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(actionExecutedContext.Exception.Message), ReasonPhrase = "ValidationException", };

				throw new HttpResponseException(response);
			}
			else if (exceptionType == typeof(UnauthorizedAccessException))
			{
				throw new HttpResponseException(actionExecutedContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new ServiceStatus() { StatusCode = (int)HttpStatusCode.Unauthorized, StatusMessage = "UnAuthorized", ReasonPhrase = "UnAuthorized Access" }));
			}
			else if (exceptionType == typeof(ApiException))
			{
				var webApiException = actionExecutedContext.Exception as ApiException;
				if (webApiException != null)
				{
					throw new HttpResponseException(actionExecutedContext.Request.CreateResponse(webApiException.HttpStatus, new ServiceStatus() { StatusCode = webApiException.ErrorCode, StatusMessage = webApiException.ErrorDescription, ReasonPhrase = webApiException.ReasonPhrase }));
				}
			}
			else if (exceptionType == typeof(ApiBusinessException))
			{
				var businessException = actionExecutedContext.Exception as ApiBusinessException;
				if (businessException != null)
				{
					throw new HttpResponseException(actionExecutedContext.Request.CreateResponse(businessException.HttpStatus, new ServiceStatus() { StatusCode = businessException.ErrorCode, StatusMessage = businessException.ErrorDescription, ReasonPhrase = businessException.ReasonPhrase }));
				}
			}
			else if (exceptionType == typeof(ApiDataException))
			{
				var dataException = actionExecutedContext.Exception as ApiDataException;
				if (dataException != null)
				{
					throw new HttpResponseException(actionExecutedContext.Request.CreateResponse(dataException.HttpStatus, new ServiceStatus() { StatusCode = dataException.ErrorCode, StatusMessage = dataException.ErrorDescription, ReasonPhrase = dataException.ReasonPhrase }));
				}
			}
			else
			{
				throw new HttpResponseException(actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError));
			}
		}
	}
}