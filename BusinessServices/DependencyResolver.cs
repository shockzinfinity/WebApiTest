using System.ComponentModel.Composition;

namespace BusinessServices
{
	[Export(typeof(Resolver.IComponent))]
	public class DependencyResolver : Resolver.IComponent
	{
		#region Resolver.IComponent implementation

		public void Setup(Resolver.IRegisterComponent registerComponent)
		{
			registerComponent.RegisterType<IProductServices, ProductServices>();
			registerComponent.RegisterType<IUserServices, UserServices>();
			registerComponent.RegisterType<ITokenServices, TokenServices>();
		}

		#endregion
	}
}
