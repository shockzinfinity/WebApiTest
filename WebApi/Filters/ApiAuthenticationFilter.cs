using System.Threading;
using System.Web.Http.Controllers;
using BusinessServices;

namespace WebApi.Filters
{
	public class ApiAuthenticationFilter : GenericAuthenticationFilter
	{
		public ApiAuthenticationFilter()
		{
		}

		public ApiAuthenticationFilter(bool isActive) : base(isActive)
		{
		}

		protected override bool OnAuthorizeUser(string user, string pass, HttpActionContext actionContext)
		{
			var provider = actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(IUserServices)) as IUserServices;

			if (provider != null)
			{
				var userId = provider.Authentication(user, pass);
				if (userId > 0)
				{
					var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
					if (basicAuthenticationIdentity != null)
						basicAuthenticationIdentity.UserId = userId;

					return true;
				}
			}

			return false;
		}
	}
}