using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TCObjectStorageClient.IO;
using TCObjectStorageClient.Properties;

namespace TCObjectStorageClient
{
    public class WindowsPreferences : IPreferences
    {
        PropertyInfo GetProperty(Settings settings, string key)
        {
            var type = settings.GetType();
            return type.GetProperty(key);
        }

        public string GetString(string key, string defaultValue)
        {
            var property = GetProperty(Settings.Default, key);
            var value = property.GetValue(Settings.Default) as string;
            return value ?? defaultValue;
        }

        public void SetString(string key, string value)
        {
            var property = GetProperty(Settings.Default, key);
            property.SetValue(Settings.Default, value);
            Settings.Default.Save();
        }
    }
}
