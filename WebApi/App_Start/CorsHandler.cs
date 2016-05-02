using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.App_Start
{
	[Obsolete("CORS 적용은 ASP.NET WebAPI 2의 CORS 를 사용하기로 합니다.")]
	public class CorsHandler : DelegatingHandler
	{
		private const string Origin = "Origin";
		private const string AccessControlRequestMethod = "Access-Control-Request-Method";
		private const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
		private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
		private const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
		private const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			bool isCorsRequest = request.Headers.Contains(Origin);
			bool isPreflightRequest = request.Method == HttpMethod.Options;

			if (isCorsRequest)
			{
				if (isPreflightRequest)
				{
					var response = new HttpResponseMessage(HttpStatusCode.OK);
					response.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());

					string accessControlRequestMethod = request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
					if (accessControlRequestMethod != null)
					{
						response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);
					}

					string requestedHeaders = string.Join(", ", request.Headers.GetValues(AccessControlRequestHeaders));
					if (!string.IsNullOrEmpty(requestedHeaders))
					{
						response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
					}

					var tcs = new TaskCompletionSource<HttpResponseMessage>();
					tcs.SetResult(response);

					return tcs.Task;
				}

				return base.SendAsync(request, cancellationToken).ContinueWith(t =>
				{
					HttpResponseMessage responseMessage = t.Result;
					responseMessage.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
					return responseMessage;
				});
			}

			return base.SendAsync(request, cancellationToken);
		}
	}
}