using System;
namespace SmartLock.Model
{
    public class WebApiEnvironment
    {
        public WebApiEnvironment(string name, string uriFormat)
        {
            Name = name;
            UriFormat = uriFormat;
        }

        public string Name { get; set; }

        public string UriFormat { get; set; }
    }
}