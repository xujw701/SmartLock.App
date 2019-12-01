using System;
using System.Net.Http;

namespace SmartLock.Model.Server
{
    public class WebServiceClientException : Exception
    {
        public WebServiceClientException(string message, Exception ex, HttpResponseMessage response) : base(message, ex)
        {
            Response = response;
        }

        public HttpResponseMessage Response { get; }
    }
}