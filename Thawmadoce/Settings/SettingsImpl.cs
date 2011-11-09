using System;
using System.Diagnostics;
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
            if (!IsSetUp) return default(T);
            var fullPath = FullPath(key);
            if (!File.Exists(fullPath))
                return default(T);

            using (var tx = File.OpenText(fullPath))
            using (var r = new JsonTextReader(tx))
                return _serializer.Deserialize<T>(r);
        }

        public T Get<T>(string key, T templateObject)
        {
            return Get<T>(key);
        }

        private string FullPath(string key)
        {
            return Path.Combine(_rootDirectory, key + ".json");
        }

        public void Post(string key, object settings)
        {
            if (!IsSetUp)
                throw new InvalidOperationException("Settings is not correctly set up!");
            var fullPath = FullPath(key);
            using (var tx = File.CreateText(fullPath))
            using (var w = new JsonTextWriter(tx))
                _serializer.Serialize(w, settings);
        }

        public void Delete(string key)
        {
            File.Delete(FullPath(key));
        }

        public bool IsSetUp
        {
            get
            {
                if (_rootDirectory == null)
                    Debug.WriteLine("Settings is not correctly set up");
                return _rootDirectory != null;
            }
        }

        void ISettingsInitializer.SetRoot(string rootDirectory)
        {
            _rootDirectory = rootDirectory;
            if (!Directory.Exists(_rootDirectory))
                Directory.CreateDirectory(_rootDirectory);
        }
    }
}