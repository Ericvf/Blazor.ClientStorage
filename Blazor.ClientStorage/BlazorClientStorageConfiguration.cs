namespace Blazor.ClientStorage
{
    public class BlazorClientStorageConfiguration : IBlazorClientStorageConfiguration
    {
        public string DatabaseName { get; set; }

        public int DatabaseVersion { get; set; }

        public string RuntimeObject { get; set; }
    }
}