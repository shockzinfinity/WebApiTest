using System;
using System.Net;
using System.Runtime.Serialization;

namespace WebApi.ErrorHelper
{
	[Serializable]
	[DataContract]
	public class ApiException : Exception, IApiExceptions
	{
		[DataMember]
		public int ErrorCode { get; set; }
		[DataMember]
		public string ErrorDescription { get; set; }
		[DataMember]
		public HttpStatusCode HttpStatus { get; set; }

		string _reasonPhrase = "ApiException";
		[DataMember]
		public string ReasonPhrase { get { return _reasonPhrase; }set { _reasonPhrase = value; } }
	}
}