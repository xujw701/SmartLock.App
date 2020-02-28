using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLock.Model.Models
{
    public class Cache
    {
        public int Id { get; }
        public string NativePath { get; private set; }
        public string Url { get; }

        public Cache(string nativePath)
        {
            NativePath = nativePath;
        }

        public Cache(int id, string url)
        {
            Id = id;
            Url = url;
            NativePath = string.Empty;
        }

        public void SetNativePath(string path)
        {
            NativePath = path;
        }
    }
}
