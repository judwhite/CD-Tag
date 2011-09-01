using System;
using System.IO;
using System.Text;
using CDTag.Common.Json;

namespace CDTag.Common.Settings
{
    /// <summary>
    /// Settings file static class for saving and loading settings.
    /// </summary>
    public static class SettingsFile
    {
        /// <summary>Saves the settings to the specified file name.</summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="settings">The settings.</param>
        public static void Save(string fileName, object settings)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException("fileName");
            if (settings == null)
                throw new ArgumentNullException("settings");

            fileName = GetFullFileName(fileName);

            string json = JsonSerializer.SerializeObject(settings);

            using (Stream fileStream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        private static string GetFullFileName(string fileName)
        {
            return Path.Combine(IoC.Resolve<IApp>().LocalApplicationDirectory, fileName);
        }

        /// <summary>Tries to load settings from the specified file.</summary>
        /// <typeparam name="T">The type to deserialize.</typeparam>
        /// <param name="fileName">The file name.</param>
        /// <param name="settings">The settings.</param>
        /// <returns><c>true</c> if the settings were loaded successfully; otherwise, <c>false</c>.</returns>
        public static bool TryLoad<T>(string fileName, out T settings)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException("fileName");

            settings = null;

            fileName = GetFullFileName(fileName);

            if (!File.Exists(fileName))
                return false;

            try
            {
                byte[] jsonBytes = File.ReadAllBytes(fileName);
                using (MemoryStream memoryStream = new MemoryStream(jsonBytes))
                {
                    settings = JsonSerializer.ReadObject<T>(memoryStream);
                }
            }
            catch
            {
                // TODO: Log error?
                return false;
            }

            return true;
        }
    }
}
