using System;
using System.IO;
using SmartLock.Model.Services;
using Foundation;
using Newtonsoft.Json;

namespace SmartLock.Presentation.iOS.Platform
{
    public class SettingsService : ISettingsService
    {
        private string CreateFilePath(string key)
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(documents, key + ".json");
        }

        public T LoadObject<T>(string key) where T : class
        {
            var filename = CreateFilePath(key);

            if (!File.Exists(filename))
            {
                return null;
            }

            var contents = File.ReadAllText(filename);

            return JsonConvert.DeserializeObject<T>(contents);
        }

        public void SaveConfiguration()
        {
            throw new NotImplementedException();
        }

        public void Save(string key, string value)
        {
            if (value == null) throw new Exception("No null values!");
            NSUserDefaults.StandardUserDefaults.SetString(value, key);
            NSUserDefaults.StandardUserDefaults.Synchronize();
        }

        public void Save(string key, int value)
        {
            NSUserDefaults.StandardUserDefaults.SetInt(value, key);
            NSUserDefaults.StandardUserDefaults.Synchronize();
        }

        public string LoadString(string key)
        {
            return NSUserDefaults.StandardUserDefaults.StringForKey(key);
        }

        public int LoadInt(string key)
        {
            return (int)NSUserDefaults.StandardUserDefaults.IntForKey(key);
        }

        public void SaveObject<T>(string key, T value) where T : class
        {
            if (value == null) throw new Exception("No null values!");
            var filename = CreateFilePath(key);

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            var contents = JsonConvert.SerializeObject(value);
            File.WriteAllText(filename, contents);
        }

        public void DeleteObject(string key)
        {
            var filename = CreateFilePath(key);

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }
    }
}
