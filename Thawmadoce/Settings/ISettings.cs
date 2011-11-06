namespace Thawmadoce.Settings
{
    public interface ISettings
    {
        T Get<T>(string key);
        void Post(string key, object settings);
    }

    internal interface ISettingsInitializer
    {
        void SetRoot(string rootDirectory);
    }
}