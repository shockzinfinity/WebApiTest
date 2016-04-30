﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Cors;
using WebApi.Attributes;

namespace WebApi.Factory
{
	public class CorsPolicyFactory : ICorsPolicyProviderFactory
	{
		ICorsPolicyProvider _provider = new CustomCorsPolicyAttribute();

		#region ICorsPolicyProviderFactory implementation

		public ICorsPolicyProvider GetCorsPolicyProvider(HttpRequestMessage request)
		{
			return _provider;
		}

		#endregion
	}
}