using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resolver
{
	public interface IRegisterComponent
	{
		void RegisterType<TFrom, TTo>(bool withInterception = false) where TTo : TFrom;
		void RegisterTypeWithControlledLifeTime<TFrom, TTo>(bool withInterception = false) where TTo : TFrom;
	}
}
