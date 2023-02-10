namespace Blazor.ClientStorage.Samples.Database;

public class LocationObjectStore : KeyPathObjectStore<Location, string>
{
    public LocationObjectStore(IBlazorClientStorage blazorClientStorage)
        : base(blazorClientStorage)
    {
    }

    public override string Name => nameof(Location);

    public override ObjectStoreDescriptor GetObjectStoreDescriptor() =>
        new()
        {
            Name = Name,
            Options = new()
            {
                KeyPath = nameof(Person.name)
            }
        };
}

public class Location : IObjectStoreModel<string>
{
    // use lowercase names
    public string key { get; set; }

#pragma warning disable IDE1006 // Naming Styles
    public string name { get; set; }
#pragma warning restore IDE1006 // Naming Styles
}