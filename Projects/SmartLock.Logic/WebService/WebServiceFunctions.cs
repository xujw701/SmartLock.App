﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartLock.Logic.Services.WebUtilities;
using SmartLock.Model.Models;
using SmartLock.Model.PushNotification;
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

        public async Task<bool> Auth(TokenPostDto tokenPostDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"users/auth");

            var result = await new WebServiceClient(_userSession).PostAsync<DefaultCreatedPostResponseDto>(uri, tokenPostDto);

            if (result != null)
            {
                return result.Id > 0;
            }
            return false;
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

        public async Task<int> UpdatePortrait(byte[] data)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, "users/portrait");

            var result = await new WebServiceClient(_userSession).PostRawAsync<DefaultCreatedPostResponseDto>(uri, data);

            if (result != null)
            {
                return result.Id;
            }
            return 0;
        }

        public async Task<byte[]> GetPortrait(int portraitId)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"users/portrait/{portraitId}");

            var result = await new WebServiceClient(_userSession).GetAsync<byte[]>(uri);

            return result;
        }

        public async Task<Keybox> GetKeybox(int? keyboxId = null, string uuid = null)
        {
            var parameters = string.Empty;

            if (keyboxId.HasValue)
            {
                parameters += $"?keyboxId={keyboxId.Value}";
            }
            else
            {
                if (!string.IsNullOrEmpty(uuid)) parameters += $"?uuid={uuid}";
            }

            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes{parameters}");

            var keyboxDto =  await new WebServiceClient(_userSession).GetAsync<KeyboxGetResponseDto>(uri);

            if (keyboxDto != null)
            {
                return new Keybox()
                {
                    KeyboxId = keyboxDto.KeyboxId,
                    CompanyId = keyboxDto.CompanyId,
                    BranchId = keyboxDto.BranchId,
                    UserId = keyboxDto.UserId,
                    Uuid = keyboxDto.Uuid,
                    PropertyId = keyboxDto.PropertyId,
                    PropertyAddress = keyboxDto.PropertyAddress,
                    KeyboxName = keyboxDto.KeyboxName,
                    BatteryLevel = keyboxDto.BatteryLevel,
                };
            }

            return null;
        }

        public async Task<List<Keybox>> GetMyKeybox()
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/mine");

            var keyboxListDto = await new WebServiceClient(_userSession).GetAsync<List<KeyboxGetResponseDto>>(uri);

            if (keyboxListDto != null)
            {
                return keyboxListDto.Select(dto => new Keybox()
                {
                    KeyboxId = dto.KeyboxId,
                    CompanyId = dto.CompanyId,
                    BranchId = dto.BranchId,
                    UserId = dto.UserId,
                    Uuid = dto.Uuid,
                    PropertyId = dto.PropertyId,
                    PropertyAddress = dto.PropertyAddress,
                    KeyboxName = dto.KeyboxName,
                    BatteryLevel = dto.BatteryLevel,
                }).ToList();
            }

            return null;
        }

        public async Task<List<Keybox>> GetKeyboxIUnlocked()
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/iunlocked");

            var keyboxListDto = await new WebServiceClient(_userSession).GetAsync<List<KeyboxGetResponseDto>>(uri);

            if (keyboxListDto != null)
            {
                return keyboxListDto.Select(dto => new Keybox()
                {
                    KeyboxId = dto.KeyboxId,
                    CompanyId = dto.CompanyId,
                    BranchId = dto.BranchId,
                    UserId = dto.UserId,
                    Uuid = dto.Uuid,
                    PropertyId = dto.PropertyId,
                    PropertyAddress = dto.PropertyAddress,
                    KeyboxName = dto.KeyboxName,
                    BatteryLevel = dto.BatteryLevel,
                    AcessUserId = dto.AcessUserId,
                    InOn = dto.InOn
                }).ToList();
            }

            return null;
        }



        public async Task<DefaultCreatedPostResponseDto> CreateKeyboxProperty(int keyboxId, KeyboxPropertyPostPutDto keyboxPropertyPostDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property");

            return await new WebServiceClient(_userSession).PostAsync<DefaultCreatedPostResponseDto>(uri, keyboxPropertyPostDto);
        }

        public async Task<Property> GetKeyboxProperty(int keyboxId, int propertyId)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property/{propertyId}");

            var propertyDto = await new WebServiceClient(_userSession).GetAsync<PropertyGetResponseDto>(uri);

            if (propertyDto != null)
            {
                return new Property()
                {
                    PropertyId = propertyDto.PropertyId,
                    PropertyName = propertyDto.PropertyName,
                    Address = propertyDto.Address,
                    Notes = propertyDto.Notes,
                    Price = propertyDto.Price,
                    Bedrooms = propertyDto.Bedrooms,
                    Bathrooms = propertyDto.Bathrooms,
                    FloorArea = propertyDto.FloorArea,
                    LandArea = propertyDto.LandArea
                };
            }

            return null;
        }

        public async Task UpdateKeyboxPin(int keyboxId, KeyboxPinPutDto keyboxPinPutDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/pin");

            await new WebServiceClient(_userSession).PutAsync(uri, keyboxPinPutDto);
        }

        public async Task UpdateKeyboxProperty(int keyboxId, int propertyId, KeyboxPropertyPostPutDto keyboxPropertyPutDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property/{propertyId}");

            await new WebServiceClient(_userSession).PutAsync(uri, keyboxPropertyPutDto);
        }

        public async Task EndKeyboxProperty(int keyboxId, int propertyId)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property/{propertyId}");

            await new WebServiceClient(_userSession).DeleteAsync(uri);
        }

        public async Task<bool> Unlock(int keyboxId, KeyboxHistoryPostDto keyboxHistoryPostDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/unlock");

            var result = await new WebServiceClient(_userSession).PostAsync<LockUnlockResponseDto>(uri, keyboxHistoryPostDto);

            if (result != null)
            {
                return result.Success;
            }

            return false;
        }

        public async Task<bool> Lock(int keyboxId, KeyboxHistoryPostDto keyboxHistoryPostDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/lock");

            var result = await new WebServiceClient(_userSession).PostAsync<LockUnlockResponseDto>(uri, keyboxHistoryPostDto);

            if (result != null)
            {
                return result.Success;
            }

            return false;
        }

        public async Task<List<KeyboxHistory>> GetHistories(int keyboxId, int propertyId)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property/{propertyId}/histories");

            var keyboxHistoryListDto = await new WebServiceClient(_userSession).GetAsync<List<KeyboxHistoryGetResponseDto>>(uri);

            if (keyboxHistoryListDto != null)
            {
                return keyboxHistoryListDto.Select(dto => new KeyboxHistory()
                {
                    KeyboxHistoryId = dto.KeyboxHistoryId,
                    KeyboxId = dto.KeyboxId,
                    UserId = dto.UserId,
                    PropertyId = dto.PropertyId,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    ResPortraitId = dto.ResPortraitId,
                    InOn = dto.InOn,
                    OutOn = dto.OutOn
                }).ToList();
            }

            return new List<KeyboxHistory>();
        }

        public async Task CreatePropertyFeedback(int keyboxId, int propertyId, FeedbackPostDto feedbackPostDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property/{propertyId}/feedback");

            await new WebServiceClient(_userSession).PostAsync(uri, feedbackPostDto);
        }

        public async Task<List<PropertyFeedback>> GetPropertyFeedback(int keyboxId, int propertyId)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property/{propertyId}/feedback");

            var propertyFeedbacksDto = await new WebServiceClient(_userSession).GetAsync<List<PropertyFeedbackGetResponseDto>>(uri);

            if (propertyFeedbacksDto != null)
            {
                return propertyFeedbacksDto.Select(dto => new PropertyFeedback()
                {
                    PropertyFeedbackId = dto.PropertyFeedbackId,
                    PropertyId = dto.PropertyId,
                    UserId = dto.UserId,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Phone = dto.Phone,
                    ResPortraitId = dto.ResPortraitId,
                    Content = dto.Content,
                    CreatedOn = dto.CreatedOn,
                }).ToList();
            }

            return new List<PropertyFeedback>();
        }

        public async Task CreateFeedback(FeedbackPostDto feedbackPostDto)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"feedback");

            await new WebServiceClient(_userSession).PostAsync(uri, feedbackPostDto);
        }

        public async Task<List<ResProperty>> GetPropertyResources(int keyboxId, int propertyId)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property/{propertyId}/resources");

            return await new WebServiceClient(_userSession).GetAsync<List<ResProperty>>(uri);
        }

        public async Task AddPropertyResource(int keyboxId, int propertyId, byte[] data)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property/{propertyId}/resources");

            await new WebServiceClient(_userSession).PostRawAsync(uri, data);
        }

        public async Task DeletePropertyResource(int keyboxId, int propertyId, int resPropertyId)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/{keyboxId}/property/{propertyId}/resources/{resPropertyId}");

            await new WebServiceClient(_userSession).DeleteAsync(uri);
        }

        public async Task<byte[]> GetPropertyResourceData(int resPropertyId)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"keyboxes/property/resources/{resPropertyId}");

            var result = await new WebServiceClient(_userSession).GetAsync<byte[]>(uri);

            return result;
        }

        public async Task<string> RequestNotificationRegistrationId(NotificationHandle notificationHandle)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"notification");
            return await new WebServiceClient(_userSession).PostAsync<string>(uri, notificationHandle);
        }

        public async Task UpsertRegistration(string registrationId, DeviceRegistration deviceRegistration)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"notification/{registrationId}");
            await new WebServiceClient(_userSession).PutAsync(uri, deviceRegistration);
        }

        public async Task RemoveRegistration(string registrationId)
        {
            var uri = _environmentManager.FormatUriForSelectedEnvironment(APIACTION, $"notification/{registrationId}");
            await new WebServiceClient().DeleteAsync(uri);
        }
    }
}
