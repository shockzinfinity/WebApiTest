using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApi.ActionFilters;

namespace WebApi
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API 구성 및 서비스

			// Web API 경로
			config.MapHttpAttributeRoutes();

			// Add logging filter attribute.
			config.Filters.Add(new LoggingFilterAttribute());
			config.Filters.Add(new GlobalExceptionAttribute());

			// 지정한 라우팅 외에 다른 기본 라우팅 제거를 위해 아래를 주석처리
			//config.Routes.MapHttpRoute(
			//	name: "DefaultApi", // name of default route
			//	routeTemplate: "api/{controller}/{id}", // pattern or representation of URL
			//	defaults: new { id = RouteParameter.Optional } // optional parameter
			//);
		}
	}
}
