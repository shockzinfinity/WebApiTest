using System.ComponentModel.Composition;
using DataModel.UnitOfWork;

namespace DataModel
{
	[Export(typeof(Resolver.IComponent))]
	public class DependencyResolver : Resolver.IComponent
	{
		#region Resolver.IComponent implementation

		public void Setup(Resolver.IRegisterComponent registerComponent)
		{
			registerComponent.RegisterType<IUnitOfWork, UnitOfWork.UnitOfWork>();
		}

		#endregion
	}
}
