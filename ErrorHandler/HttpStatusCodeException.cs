using System;
using System.Net;
using Newtonsoft.Json.Linq;

namespace EmpDepAPI.ErrorHandler
{
    public class HttpStatusCodeException : Exception
    {
        
        public HttpStatusCode StatusCode { get; set; }
        public string ContentType { get; set; } = @"text/plain";

        public HttpStatusCodeException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
        public HttpStatusCodeException(HttpStatusCode statusCode, string message):base(message)
        {
            StatusCode = statusCode;
        }
        public HttpStatusCodeException(HttpStatusCode statusCode, Exception inner):this(statusCode,inner.ToString())
        {
            StatusCode = statusCode;
        }
         public HttpStatusCodeException(HttpStatusCode statusCode, JObject errorObject):this(statusCode,errorObject.ToString())
        {
            StatusCode = statusCode;
            ContentType = @"application/json";
        }
        
    }
}