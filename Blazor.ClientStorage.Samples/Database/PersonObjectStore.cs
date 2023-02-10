namespace Blazor.ClientStorage.Samples.Database;

public class PersonObjectStore : AutoIncrementObjectStore<Person>
{
    public PersonObjectStore(IBlazorClientStorage blazorClientStorage)
        : base(blazorClientStorage)
    {
    }

    public override string Name => nameof(Person);

    public override ObjectStoreDescriptor GetObjectStoreDescriptor() =>
        new()
        {
            Name = Name,
            Options = new()
            {
                AutoIncrement = true
            }
        };
}

public class Person : IObjectStoreModel<int>
{
    // use lowercase names
    public int key { get; set; }

#pragma warning disable IDE1006 // Naming Styles
    public string name { get; set; }

    public int age { get; set; }
#pragma warning restore IDE1006 // Naming Styles
}