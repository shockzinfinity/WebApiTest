using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace WebApi.ErrorHelper
{
	public interface IApiExceptions
	{
		int ErrorCode { get; set; }
		string ErrorDescription { get; set; }
		HttpStatusCode HttpStatus { get; set; }
		string ReasonPhrase { get; set; }
	}
}