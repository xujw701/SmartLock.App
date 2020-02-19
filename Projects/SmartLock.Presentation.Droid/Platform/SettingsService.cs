using System;
using SmartLock.Model.Services;
using System.IO;
using Newtonsoft.Json;
using Android.App;
using Android.Content;

namespace SmartLock.Presentation.Droid.Platform
{
    public class SettingsService : ISettingsService
    {
        private const string AndroidSettingsFilePath = "SmartLock.Settings";

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

        public string LoadString(string key)
        {
            var preferences = Application.Context.GetSharedPreferences(AndroidSettingsFilePath, FileCreationMode.Private);
            return preferences.GetString(key, null);
        }

        public void Save(string key, int value)
        {
            var preferences = Application.Context.GetSharedPreferences(AndroidSettingsFilePath, FileCreationMode.Private);
            var preferencesEditor = preferences.Edit();
            preferencesEditor.PutInt(key, value);
            preferencesEditor.Commit();
        }

        public int LoadInt(string key)
        {
            var preferences = Application.Context.GetSharedPreferences(AndroidSettingsFilePath, FileCreationMode.Private);
            return preferences.GetInt(key, 0);
        }

        public void Save(string key, string value)
        {
            if (value == null) throw new Exception("No null values!");
            var preferences = Application.Context.GetSharedPreferences(AndroidSettingsFilePath, FileCreationMode.Private);
            var preferencesEditor = preferences.Edit();
            preferencesEditor.PutString(key, value);
            preferencesEditor.Commit();
        }

        public void SaveConfiguration()
        {
            // throw new NotImplementedException();
        }

        public void SaveObject<T>(string key, T value) where T : class
        {
            if (value == null) throw new Exception("No null values!");
            var filename = CreateFilePath(key);

            if (System.IO.File.Exists(filename))
            {
                File.Delete(filename);
            }

            var contents = JsonConvert.SerializeObject(value);
            File.WriteAllText(filename, contents);
        }

        public void DeleteObject(string key)
        {
            var filename = CreateFilePath(key);

            if (System.IO.File.Exists(filename))
            {
                File.Delete(filename);
            }
        }
    }
}