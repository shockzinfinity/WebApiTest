using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using BusinessServices;
using DataModel.UnitOfWork;
using Microsoft.Practices.Unity;

namespace WebApi
{
	public static class Bootstrapper
	{
		public static void Initialize()
		{
			var container = BuildUnityContainer();

			// UnityDependencyResolver 가 이름은 같지만, Microsoft.Practices.Unity.Mvc.UnityDependencyResolver 와 
			// Unity.WebApi.UnityDependencyResolver 는 다른 타입이다.
			// Mvc DependencyResolver 에는 Microsoft.Practices.Unity.Mvc.UnityDependencyResolver 를 넣어야 하고
			// GlobalConfiguration 에는 Unity.WebApi.UnityDependencyResolver 를 넣어야 한다.

			DependencyResolver.SetResolver(new Microsoft.Practices.Unity.Mvc.UnityDependencyResolver(container));

			// register dependency resolver for WebAPI
			GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
		}

		private static IUnityContainer BuildUnityContainer()
		{
			var container = new UnityContainer();

			// register all your components with the container here
			// it is NOT necessary to register your controllers
			// e.g. container.RegisterType<ITestService, TestService>();

			container.RegisterType<IProductServices, ProductServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());

			return container;
		}
	}
}
