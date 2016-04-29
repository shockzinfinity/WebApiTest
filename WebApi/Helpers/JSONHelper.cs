using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Helpers
{
	public static class JSONHelper
	{
		public static string ToJSON(this object obj)
		{
			var serializer = new JavaScriptSerializer();
		}
	}
}