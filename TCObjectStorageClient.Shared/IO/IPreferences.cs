namespace TCObjectStorageClient.IO
{
    public interface IPreferences
    {
        string GetString(string key, string defaultValue);
        void SetString(string key, string value);
    }
}
