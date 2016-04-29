using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApi.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class GenericAuthenticationFilter : AuthorizationFilterAttribute
	{
		private readonly bool _isActive = true;

		public GenericAuthenticationFilter()
		{
		}

		public GenericAuthenticationFilter(bool isActive)
		{
			_isActive = isActive;
		}

		public override void OnAuthorization(HttpActionContext actionContext)
		{
			if (!_isActive) return;
			var identity = FetchAuthHeader(actionContext);

			base.OnAuthorization(actionContext);
		}

		protected virtual bool OnAuthorizeUser(string user, string pass, HttpActionContext actionContext)
		{
			if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass)) return false;

			return true;
		}

		protected virtual BasicAuthenticationIdentity FetchAuthHeader(HttpActionContext actionContext)
		{
			string authHeaderValue = null;
			var authRequest = actionContext.Request.Headers.Authorization;
			if (authRequest != null && !string.IsNullOrEmpty(authRequest.Scheme) && authRequest.Scheme == "Basic")
			{
				authHeaderValue = authRequest.Parameter;
			}
			if (string.IsNullOrEmpty(authHeaderValue))
				return null;
			authHeaderValue = Encoding.Default.GetString(Convert.FromBase64String(authHeaderValue));
			var credentials = authHeaderValue.Split(':');

			return credentials.Length < 2 ? null : new BasicAuthenticationIdentity(credentials[0], credentials[1]);
		}

		private static void ChallengeAuthRequest(HttpActionContext actionContext)
		{
			var dnsHost = actionContext.Request.RequestUri.DnsSafeHost;
			actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
			actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", dnsHost));
		}
	}
}