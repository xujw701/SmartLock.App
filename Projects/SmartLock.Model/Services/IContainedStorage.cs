namespace SmartLock.Model.Services
{
    /// <summary>
    /// Stores data inside its own container (i.e. a folder) to avoid collision (between workspaces)
    /// </summary>
    public interface IContainedStorage
    {
        /// <summary>
        /// Initializes the storage given a unique key
        /// </summary>
        /// <param name="key">Unique key to be used for storing data</param>
        void Init(string key);
        
        /// <summary>
        /// Gets a serialized object from the contained storage.
        /// </summary>
        T GetSerializedObject<T>(string key) where T : class;
        
        /// <summary>
        /// Save a serialized object to the contained storage.
        /// </summary>
        void StoreObjectSerialized<T>(string key, T value) where T : class;

        void StoreRawFile(string key, byte[] data);

        string[] GetRawFileKeys();

        string[] GetSerializedObjectKeys();

        void DeleteRawFile(string key);

        byte[] GetRawFile(string key);

        byte[] GetRawFileWithPath(string nativePath);

        bool RawFileExists(string key);

        string GetRawFileNativePath(string key);

        void DeleteFile(string nativePath);

        void DeleteSerializedObject(string key);
    }
}
