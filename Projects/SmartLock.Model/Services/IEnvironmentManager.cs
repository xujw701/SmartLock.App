using System;
using System.Collections.Generic;

namespace SmartLock.Model.Services
{
    public interface IEnvironmentManager
    {
        List<WebApiEnvironment> Environments { get; }

        WebApiEnvironment SelectedEnvironment { get; set; }

        Uri FormatUriForSelectedEnvironment(string action, params object[] data);
    }
}
