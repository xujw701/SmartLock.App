using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartLock.Logic.Services.WebUtilities;
using SmartLock.Model.Request;
using SmartLock.Model.Response;
using SmartLock.Model.Services;

namespace SmartLock.Logic
{
    public class WebServiceFunctions : IWebService
    {
        private const string APIACTION = "/api/v1";

        private readonly IEnvironmentManager _environmentManager;
        private readonly IUserSession _userSession;


        public WebServiceFunctions(IEnvironmentManager environmentManager, IUserSession userSession)
        {
            _environmentManager = environmentManager;
            _userSession = userSession;
        }

        public async Task<TokenPostResponseDto> Token(TokenPostDto tokenPostDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"users/token");

            var result = await new WebServiceClient().PostAsync<TokenPostResponseDto>(uri, tokenPostDto);

            return result;
        }

        public async Task<MePostResponseDto> GetMe()
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"users/me");

            var result = await new WebServiceClient(_userSession).GetAsync<MePostResponseDto>(uri);

            return result;
        }

        public async Task UpdateMe(MePutDto mePutDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"users/me");

            await new WebServiceClient(_userSession).PutAsync(uri, mePutDto);
        }

        public async Task<KeyboxGetResponseDto> GetKeybox(int? keyboxId = null, string uuid = null)
        {
            var parameters = string.Empty;

            if (keyboxId.HasValue) parameters += $"&keyboxId={keyboxId.Value}";
            if (!string.IsNullOrEmpty(uuid)) parameters += $"&uuid={uuid}";

            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes{parameters}");

            return await new WebServiceClient(_userSession).GetAsync<KeyboxGetResponseDto>(uri);
        }

        public async Task<List<KeyboxGetResponseDto>> GetMyKeybox()
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/mine");

            return await new WebServiceClient(_userSession).GetAsync<List<KeyboxGetResponseDto>>(uri);
        }

        public async Task<DefaultCreatedPostResponseDto> CreateKeyboxProperty(int keyboxId, KeyboxPropertyPostPutDto keyboxPropertyPostDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property");

            return await new WebServiceClient(_userSession).PostAsync<DefaultCreatedPostResponseDto>(uri, keyboxPropertyPostDto);
        }

        public async Task<PropertyGetResponseDto> GetKeyboxProperty(int keyboxId, int propertyId)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property{propertyId}");

            return await new WebServiceClient(_userSession).GetAsync<PropertyGetResponseDto>(uri);
        }

        public async Task UpdateKeyboxProperty(int keyboxId, int propertyId, KeyboxPropertyPostPutDto keyboxPropertyPutDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property{propertyId}");

            await new WebServiceClient(_userSession).PutAsync(uri, keyboxPropertyPutDto);
        }

        public async Task EndKeyboxProperty(int keyboxId, int propertyId)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property{propertyId}");

            await new WebServiceClient(_userSession).DeleteAsync(uri);
        }

        public async Task<LockUnlockResponseDto> Unlock(int keyboxId, KeyboxHistoryPostDto keyboxHistoryPostDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/unlock");

            return await new WebServiceClient(_userSession).PostAsync<LockUnlockResponseDto>(uri, keyboxHistoryPostDto);
        }

        public async Task<LockUnlockResponseDto> Lock(int keyboxId, KeyboxHistoryPostDto keyboxHistoryPostDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/lock");

            return await new WebServiceClient(_userSession).PostAsync<LockUnlockResponseDto>(uri, keyboxHistoryPostDto);
        }

        public async Task<List<KeyboxHistoryGetResponseDto>> GetHistories(int keyboxId, int propertyId)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property/{propertyId}/histories");

            return await new WebServiceClient(_userSession).GetAsync<List<KeyboxHistoryGetResponseDto>>(uri);
        }

        public async Task CreatePropertyFeedback(int keyboxId, int propertyId, FeedbackPostDto feedbackPostDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property/{propertyId}/feedback");

            await new WebServiceClient(_userSession).PostAsync(uri, feedbackPostDto);
        }

        public async Task<List<PropertyFeedbackGetResponseDto>> GetPropertyFeedback(int keyboxId, int propertyId)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property/{propertyId}/feedback");

            return await new WebServiceClient(_userSession).GetAsync<List<PropertyFeedbackGetResponseDto>>(uri);
        }

        public async Task CreateFeedback(FeedbackPostDto feedbackPostDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"feedback");

            await new WebServiceClient(_userSession).PostAsync(uri, feedbackPostDto);
        }
    }
}
