using SmartLock.Model.Models;
using SmartLock.Model.Services;
using System;
using System.Linq;

namespace SmartLock.Logic.Services
{
    public class CacheManager : ICacheManager
    {
        public const string PortraitStorageKey = "PortraitStorage";

        private readonly IContainedStorage _containedStorage;
        private readonly IUserSession _userSession;

        public CacheManager(IContainedStorage containedStorage, IUserSession userSession)
        {
            _containedStorage = containedStorage;
            _userSession = userSession;
        }

        public void Init(string storageKey)
        {
            _containedStorage.Init($"{storageKey}/{_userSession.UserId}");
        }

        public Cache Save(byte[] data, string key = null)
        {
            if (key == null) key = Guid.NewGuid().ToString();
            _containedStorage.StoreRawFile(key, data);

            return Get(key);
        }

        public Cache Get(string key)
        {
            var keys = _containedStorage.GetRawFileKeys();

            if (keys.Any(k => !string.IsNullOrEmpty(k) && k.Equals(key)))
            {
                var nativePath = _containedStorage.GetRawFileNativePath(key);
                return new Cache(nativePath);
            }
            return null;
        }

        public byte[] GetRawData(string nativePath)
        {
            return _containedStorage.GetRawFileWithPath(nativePath);
        }

        public void Delete(string key)
        {
            _containedStorage.DeleteRawFile(key);
        }

        public void Clear()
        {
            var keys = _containedStorage.GetRawFileKeys();
            foreach (var key in keys)
            {
                _containedStorage.DeleteRawFile(key);
            }
        }
    }
}
