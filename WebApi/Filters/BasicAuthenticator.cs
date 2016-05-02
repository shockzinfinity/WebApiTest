using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using BusinessServices;

namespace WebApi.Filters
{
	public class BasicAuthenticator : Attribute, IAuthenticationFilter
	{
		//private readonly string _realm;

		public BasicAuthenticator(/* string realm */)
		{
			//_realm = "realm=" + realm;
		}

		#region IAuthenticationFilter implementation

		public bool AllowMultiple { get { return false; } }

		public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
		{
			var request = context.Request;
			if (request.Headers.Authorization != null && request.Headers.Authorization.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase))
			{
				Encoding encoding = Encoding.GetEncoding("iso-8859-1");
				string credentials = encoding.GetString(Convert.FromBase64String(request.Headers.Authorization.Parameter));

				string[] parts = credentials.Split(':');
				string userName = parts[0].Trim();
				string password = parts[1].Trim();

				var provider = context.ActionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(IUserServices)) as IUserServices;

				if (provider != null)
				{
					var userId = provider.Authentication(userName, password);
					if (userId > 0)
					{
						var claims = new List<Claim>()
						{
							new Claim(ClaimTypes.Name, userName),
							new Claim("userId", userId.ToString())
						};
						var id = new ClaimsIdentity(claims, "Basic");
						var principal = new ClaimsPrincipal(new[] { id });
						context.Principal = principal;
					}
				}
			}
			else
			{
				context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
			}

			return Task.FromResult(0);
		}

		public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
		{
			var dnsHost = context.ActionContext.Request.RequestUri.DnsSafeHost;

			context.Result = new ResultWithChallenge(context.Result, dnsHost);

			return Task.FromResult(0);
		}

		#endregion
	}

	public class ResultWithChallenge : IHttpActionResult
	{
		private readonly IHttpActionResult _next;
		private readonly string _realm;

		public ResultWithChallenge(IHttpActionResult next, string realm)
		{
			_next = next;
			_realm = realm;
		}

		#region IHttpActionResult implementation

		public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			var result = await _next.ExecuteAsync(cancellationToken);
			if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				result.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Basic", _realm));
			}

			return result;
		}

		#endregion
	}
}