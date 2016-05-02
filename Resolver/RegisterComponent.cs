﻿using Microsoft.Practices.Unity;

namespace Resolver
{
	internal class RegisterComponent : IRegisterComponent
	{
		private readonly IUnityContainer _container;

		public RegisterComponent(IUnityContainer container)
		{
			_container = container;
			// register interception behaviour if any
		}

		public void RegisterType<TFrom, TTo>(bool withInterception = false) where TTo : TFrom
		{
			if (withInterception)
			{
				// register with interception
			}
			else
			{
				this._container.RegisterType<TFrom, TTo>();
			}
		}

		public void RegisterTypeWithControlledLifeTime<TFrom, TTo>(bool withInterception = false) where TTo : TFrom
		{
			this._container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());
		}
	}
}
