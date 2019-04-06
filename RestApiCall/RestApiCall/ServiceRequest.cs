using System;
using System.Net.Http;

namespace RestApiCall
{
    public class ServiceRequest
    {
        public string RequestUrl { get; set; }
        public HttpContent Content { get; set; }
        public string AuthToken { get; set; }
        public RequestMethodTypes RequestMethod { get; set; }
    }
    public enum RequestMethodTypes
    {
        Get,
        Post,
        Put,
        Delete,
    }
}
