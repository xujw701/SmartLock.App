using System;
using System.IO;
using System.Linq;
using SmartLock.Model.Services;
using Newtonsoft.Json;

namespace SmartLock.Presentation.iOS.Platform
{
    public class ContainedStorage : IContainedStorage
    {
        private string _path;
        private const string SerializedFileExtension = ".json";

        private string Path
        {
            get
            {
                if (string.IsNullOrEmpty(_path))
                {
                    throw new Exception("Contained storage hasn't been initialized");
                }
                return _path;
            }
        }

        public void Init(string key)
        {
            // Get a safe key for storage
            var invalid = new string(System.IO.Path.GetInvalidFileNameChars()) + new string(System.IO.Path.GetInvalidPathChars());

            foreach (var c in invalid)
            {
                key = key.Replace(c.ToString(), "");
            }

            // Create the path
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _path = System.IO.Path.Combine(documents, key);

            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }


        public T GetSerializedObject<T>(string key) where T : class
        {
            var path = GetSerializedPathForKey(key);

            if (!File.Exists(path))
            {
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
            catch (Exception ex)
            {
                // TODO: Log
            }

            return null;
        }

        public void StoreObjectSerialized<T>(string key, T value) where T : class
        {
            var path = GetSerializedPathForKey(key);

            if (SerlializedFileExists(key))
            {
                DeleteSerializedObject(key);
            }

            if (value == null)
            {
                return;
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(value));
        }

        private string GetSerializedPathForKey(string key)
        {
            if (Path == null) throw new Exception("Call Init");

            return System.IO.Path.Combine(Path, key + SerializedFileExtension);
        }

        public bool SerlializedFileExists(string key)
        {
            return File.Exists(GetSerializedPathForKey(key));
        }

        public void DeleteSerializedObject(string key)
        {
            var fileName = GetSerializedPathForKey(key);

            if (!File.Exists(fileName))
            {
                return;
            }

            try
            {
                File.Delete(fileName);
            }
            catch (Exception ex)
            {
                // TODO: Log
            }
        }

        public string GetRawFileNativePath(string key)
        {
            if (Path == null) throw new Exception("Call Init");

            return System.IO.Path.Combine(Path, key);
        }

        public void StoreRawFile(string key, byte[] data)
        {
            if (RawFileExists(key))
            {
                DeleteRawFile(key);
            }

            File.WriteAllBytes(GetRawFileNativePath(key), data);
        }

        public byte[] GetRawFile(string key)
        {
            if (!RawFileExists(key))
            {
                return null;
            }

            return File.ReadAllBytes(GetRawFileNativePath(key));
        }

        public byte[] GetRawFileWithPath(string nativePath)
        {
            return File.ReadAllBytes(nativePath);
        }

        public bool RawFileExists(string key)
        {
            return File.Exists(GetRawFileNativePath(key));
        }

        public void DeleteRawFile(string key)
        {
            var fileName = GetRawFileNativePath(key);

            if (!File.Exists(fileName))
            {
                return;
            }

            try
            {
                File.Delete(fileName);
            }
            catch (Exception ex)
            {
                // TODO: Log
            }
        }

        public string[] GetRawFileKeys()
        {
            if (Path == null) throw new Exception("Call Init");

            var files = Directory.GetFiles(Path)
                                 .Where(f => !f.EndsWith(SerializedFileExtension))
                                 .Select(s => s.Substring(s.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1));

            return files.ToArray();
        }

        public string[] GetSerializedObjectKeys()
        {
            if (Path == null) throw new Exception("Call Init");

            var files = Directory.GetFiles(Path)
                .Where(f => f.EndsWith(SerializedFileExtension))
                .Select(s => s.Substring(s.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1))
                .Select(s => s.Substring(0, s.Length - SerializedFileExtension.Length));

            return files.ToArray();
        }


        public void DeleteFile(string nativePath)
        {
            if (!File.Exists(nativePath))
            {
                return;
            }

            try
            {
                File.Delete(nativePath);
            }
            catch (Exception ex)
            {
                // TODO: Log
            }
        }
    }
}
