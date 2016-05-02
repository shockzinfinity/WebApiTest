using System.Net;

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