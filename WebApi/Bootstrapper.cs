using System.Web.Http;
using Microsoft.Practices.Unity;
using Resolver;

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

			// DependencyResolver 라는 이름으로 이미 BusinessServices 프로젝트에서 가지고 있으므로, 명시적 참조로 변경, Using BusinessServices 주석처리
			//DependencyResolver.SetResolver(new Microsoft.Practices.Unity.Mvc.UnityDependencyResolver(container));
			System.Web.Mvc.DependencyResolver.SetResolver(new Microsoft.Practices.Unity.Mvc.UnityDependencyResolver(container));

			// register dependency resolver for WebAPI
			GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
		}

		private static IUnityContainer BuildUnityContainer()
		{
			var container = new UnityContainer();

			// register all your components with the container here
			// it is NOT necessary to register your controllers
			// e.g. container.RegisterType<ITestService, TestService>();

			// 이 부분을 MEF로 변경
			//container.RegisterType<IProductServices, ProductServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
			RegisterTypes(container); // MEF 타입 등록

			return container;
		}

		private static void RegisterTypes(UnityContainer container)
		{
			// Component initialization via MEF
			// 현재는 문자열로 등록되게 했기 때문에, 프로젝트 이름이 변경되면 Regex를 써도 된다.
			// 2016-04-28
			// MEF를 도입함으로서 아키텍쳐적으로 여러가지 이점을 챙길수 있다.
			// 1. 확장성과 loosely coupled 한 api 를 가질수 있게 된다.
			// 2. 리플렉션을 이용한 api 등록이므로, runtime 상에서 자동적으로 로딩할 수 있다. (공통의 suffix를 통해서...)
			// 3. Database transaction 등이 서비스에 노출되지 않으므로 구조적으로 보안상의 위험을 줄일수 있다.
			ComponentLoader.LoadContainer(container, ".\\bin", "WebApi.dll"); // WebApi 프로젝트에서 등록
			ComponentLoader.LoadContainer(container, ".\\bin", "BusinessServices.dll"); // BusinessServices 프로젝트에서 등록
		}
	}
}
