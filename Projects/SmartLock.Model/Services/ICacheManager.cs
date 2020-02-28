using SmartLock.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLock.Model.Services
{
    public interface ICacheManager
    {
        void Init(string storageKey);
        Cache Save(byte[] data, string key = null);
        Cache Get(string key);
        byte[] GetRawData(string key);
        void Delete(string key);
        void Clear();
    }
}
