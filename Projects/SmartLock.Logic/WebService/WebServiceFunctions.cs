using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartLock.Logic.Services.WebUtilities;
using SmartLock.Model.Services;

namespace SmartLock.Logic
{
    public class WebServiceFunctions : IWebService
    {
        private const string APIACTION = "/api";

        //private readonly IEnvironmentManager _environmentManager;
        //private readonly IUserSession _userSession;


        //public WebServiceFunctions(IEnvironmentManager environmentManager, IUserSession userSession)
        //{
        //    _environmentManager = environmentManager;
        //    _userSession = userSession;
        //}

        //public async Task<ParentLoginPostResponseDto> Login(ParentLoginPostDto loginRequest)
        //{
        //    var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, "parents/login");

        //    var result = await new WebServiceClient().PostAsync<ParentLoginPostResponseDto>(uri, loginRequest);

        //    return result;
        //}
    }
}
