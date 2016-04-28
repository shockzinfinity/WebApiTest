using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
	[Export(typeof(Resolver.IComponent))]
	public class DependencyResolver : Resolver.IComponent
	{
		#region Resolver.IComponent implementation

		public void Setup(Resolver.IRegisterComponent registerComponent)
		{
			registerComponent.RegisterType<IProductServices, ProductServices>();
		}

		#endregion
	}
}
