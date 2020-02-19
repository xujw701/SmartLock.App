using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartLock.Model.Request;
using SmartLock.Model.Response;

namespace SmartLock.Model.Services
{
    public interface IWebService
    {
        Task<TokenPostResponseDto> Token(TokenPostDto tokenPostDto);
        Task<MePostResponseDto> GetMe();
        Task UpdateMe(MePutDto mePutDto);
        Task<KeyboxGetResponseDto> GetKeybox(int? keyboxId = null, string uuid = null);
        Task<List<KeyboxGetResponseDto>> GetMyKeybox();
        Task<DefaultCreatedPostResponseDto> CreateKeyboxProperty(int keyboxId, KeyboxPropertyPostPutDto keyboxPropertyPostDto);
        Task<PropertyGetResponseDto> GetKeyboxProperty(int keyboxId, int propertyId);
        Task UpdateKeyboxProperty(int keyboxId, int propertyId, KeyboxPropertyPostPutDto keyboxPropertyPutDto);
        Task EndKeyboxProperty(int keyboxId, int propertyId);
        Task<LockUnlockResponseDto> Unlock(int keyboxId, KeyboxHistoryPostDto keyboxHistoryPostDto);
        Task<LockUnlockResponseDto> Lock(int keyboxId, KeyboxHistoryPostDto keyboxHistoryPostDto);
        Task<List<KeyboxHistoryGetResponseDto>> GetHistories(int keyboxId, int propertyId);
        Task CreatePropertyFeedback(int keyboxId, int propertyId, FeedbackPostDto feedbackPostDto);
        Task<List<PropertyFeedbackGetResponseDto>> GetPropertyFeedback(int keyboxId, int propertyId);
        Task CreateFeedback(FeedbackPostDto feedbackPostDto);
    }
}
