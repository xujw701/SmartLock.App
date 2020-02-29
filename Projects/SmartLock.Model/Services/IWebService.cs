using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartLock.Model.Models;
using SmartLock.Model.PushNotification;
using SmartLock.Model.Request;
using SmartLock.Model.Response;

namespace SmartLock.Model.Services
{
    public interface IWebService
    {
        Task<bool> Auth(TokenPostDto tokenPostDto);
        Task<TokenPostResponseDto> Token(TokenPostDto tokenPostDto);
        Task<MePostResponseDto> GetMe();
        Task UpdateMe(MePutDto mePutDto);
        Task<int> UpdatePortrait(byte[] data);
        Task<byte[]> GetPortrait(int portraitId);
        Task<Keybox> GetKeybox(int? keyboxId = null, string uuid = null);
        Task<List<Keybox>> GetMyKeybox();
        Task<List<Keybox>> GetKeyboxIUnlocked();
        Task<DefaultCreatedPostResponseDto> CreateKeyboxProperty(int keyboxId, KeyboxPropertyPostPutDto keyboxPropertyPostDto);
        Task<Property> GetKeyboxProperty(int keyboxId, int propertyId);
        Task UpdateKeyboxProperty(int keyboxId, int propertyId, KeyboxPropertyPostPutDto keyboxPropertyPutDto);
        Task EndKeyboxProperty(int keyboxId, int propertyId);
        Task<bool> Unlock(int keyboxId, KeyboxHistoryPostDto keyboxHistoryPostDto);
        Task<bool> Lock(int keyboxId, KeyboxHistoryPostDto keyboxHistoryPostDto);
        Task<List<KeyboxHistory>> GetHistories(int keyboxId, int propertyId);
        Task CreatePropertyFeedback(int keyboxId, int propertyId, FeedbackPostDto feedbackPostDto);
        Task<List<PropertyFeedback>> GetPropertyFeedback(int keyboxId, int propertyId);
        Task CreateFeedback(FeedbackPostDto feedbackPostDto);
        Task<List<ResProperty>> GetPropertyResources(int keyboxId, int propertyId);
        Task AddPropertyResource(int keyboxId, int propertyId, byte[] data);
        Task DeletePropertyResource(int keyboxId, int propertyId, int resPropertyId);
        Task<byte[]> GetPropertyResourceData(int resPropertyId);
        Task<string> RequestNotificationRegistrationId(NotificationHandle notificationHandle);
        Task UpsertRegistration(string registrationId, DeviceRegistration deviceRegistration);
        Task RemoveRegistration(string registrationId);
    }
}
