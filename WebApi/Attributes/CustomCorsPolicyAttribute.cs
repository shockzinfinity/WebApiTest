using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace WebApi.Attributes
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
	public class CustomCorsPolicyAttribute : Attribute, ICorsPolicyProvider
	{
		private CorsPolicy _policy;

		public CustomCorsPolicyAttribute()
		{
			_policy = new CorsPolicy
			{
				AllowAnyMethod = true,
				AllowAnyHeader = true
			};

			// add allowed origins.
			// web.config 에서 읽어와서 지정해도 된다.
			_policy.Origins.Add("http://localhost");
		}

		#region ICorsPolicyProvider implementation

		public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return Task.FromResult(_policy);
		}

		#endregion
	}
}