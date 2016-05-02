using System;
using System.Net;
using System.Runtime.Serialization;

namespace WebApi.ErrorHelper
{
	[Serializable]
	[DataContract]
	public class ApiDataException : Exception, IApiExceptions
	{
		[DataMember]
		public int ErrorCode { get; set; }
		[DataMember]
		public string ErrorDescription { get; set; }
		[DataMember]
		public HttpStatusCode HttpStatus { get; set; }

		string _reasonPhrase = "ApiDataException";
		[DataMember]
		public string ReasonPhrase { get { return _reasonPhrase; } set { _reasonPhrase = value; } }

		public ApiDataException(int errorCode, string errorDescription, HttpStatusCode httpStatus)
		{
			ErrorCode = errorCode;
			ErrorDescription = errorDescription;
			HttpStatus = httpStatus;
		}
	}
}