using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApi.App_Start;

namespace WebApi
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			// initialize bootstrapper
			Bootstrapper.Initialize();

			// define formatters
			var formatters = GlobalConfiguration.Configuration.Formatters;
			var jsonFormatter = formatters.JsonFormatter;
			var settings = jsonFormatter.SerializerSettings;
			settings.Formatting = Formatting.Indented;
			//settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			var appXmlType = formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
			formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

			// CORS handler
			GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsHandler());
			// TODO: ASP.NET CORS support 로 변경
			// http://www.asp.net/web-api/overview/security/enabling-cross-origin-requests-in-web-api
		}
	}
}
