namespace SmartLock.Model.Services
{
	public interface ISettingsService
    {
        void Save(string key, string value);

        string LoadString(string key);

        void Save(string key, int value);

        int LoadInt(string key);

        void SaveObject<T>(string key, T value) where T : class;

        T LoadObject<T>(string key) where T : class;

        void DeleteObject(string key);
    }
}
