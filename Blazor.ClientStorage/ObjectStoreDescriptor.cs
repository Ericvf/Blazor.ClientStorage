namespace Blazor.ClientStorage
{
    public class ObjectStoreDescriptor
    {
        public string Name { get; set; }

        public OptionsDescriptor Options { get; set; }

        public IndexDescriptor[] Indices { get; set; }

       
    }

    public class OptionsDescriptor
    {
        public string keyPath { get; set; }

        public bool autoIncrement { get; set; }

    }

    public class IndexDescriptor
    {
        public string Name { get; set; }

        public IndexOptionsDescriptor Options { get; set; }
    }

    public class IndexOptionsDescriptor
    {
        public bool unique { get; set; }
    }
}
