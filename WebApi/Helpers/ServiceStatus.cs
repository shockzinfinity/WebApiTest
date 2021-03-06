﻿using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace WebApi.Helpers
{
	[Serializable]
	[DataContract]
	public class ServiceStatus
	{
		[JsonProperty("StatusCode")]
		[DataMember]
		public int StatusCode { get; set; }
		[JsonProperty("StatusMessage")]
		[DataMember]
		public string StatusMessage { get; set; }
		[JsonProperty("ReasonPhrase")]
		[DataMember]
		public string ReasonPhrase { get; set; }
	}
}