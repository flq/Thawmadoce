using System.IO;
using Newtonsoft.Json;

namespace Thawmadoce.Settings
{
    public class SettingsImpl : ISettings, ISettingsInitializer
    {
        private string _rootDirectory;
        private readonly JsonSerializer _serializer;

        public SettingsImpl()
        {
            _serializer = new JsonSerializer();
        }

        public T Get<T>(string key)
        {
            var fullPath = FullPath(key);
            if (!File.Exists(fullPath))
                return default(T);

            using (var tx = File.OpenText(fullPath))
            using (var r = new JsonTextReader(tx))
                return _serializer.Deserialize<T>(r);
        }

        private string FullPath(string key)
        {
            return Path.Combine(_rootDirectory, key + ".json");
        }

        public void Post(string key, object settings)
        {
            var fullPath = FullPath(key);

            using (var tx = File.CreateText(fullPath))
            using (var w = new JsonTextWriter(tx))
                _serializer.Serialize(w, settings);
        }

        void ISettingsInitializer.SetRoot(string rootDirectory)
        {
            _rootDirectory = rootDirectory;
            if (!Directory.Exists(_rootDirectory))
                Directory.CreateDirectory(_rootDirectory);
        }
    }
}