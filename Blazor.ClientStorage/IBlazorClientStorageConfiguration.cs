namespace Blazor.ClientStorage
{
    public interface IBlazorClientStorageConfiguration
    {
        string DatabaseName { get; set; }

        int DatabaseVersion { get; set; }

        string RuntimeObject { get; set; }
    }
}