using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using BusinessServices;

namespace WebApi.ActionFilters
{
	public class AuthorizationRequiredAttribute : ActionFilterAttribute
	{
		private const string _token = "Token";

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			// GET API key provider
			var provider = actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(ITokenServices)) as ITokenServices;

			if (actionContext.Request.Headers.Contains(_token))
			{
				var tokenValue = actionContext.Request.Headers.GetValues(_token).First();

				// validate token
				if (provider != null && !provider.ValidateToken(tokenValue))
				{
					var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Request" };
					actionContext.Response = responseMessage;
				}
			}
			else
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
			}

			base.OnActionExecuting(actionContext);
		}
	}
}