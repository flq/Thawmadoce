namespace Thawmadoce.Settings
{
    public interface ISettings
    {
        T Get<T>(string key);
        /// <summary>
        /// Helps you get back values stored as anonymous object
        /// </summary>
        T Get<T>(string key, T templateObject);
        void Post(string key, object settings);
        void Delete(string key);

        bool IsSetUp { get; }
    }

    internal interface ISettingsInitializer
    {
        void SetRoot(string rootDirectory);
    }
}