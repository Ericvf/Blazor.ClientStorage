# What is  Blazor ClientStorage

Blazor ClientStorage is a thin wrapper around [IndexedDB](https://developer.mozilla.org/en-US/docs/Web/API/IndexedDB_API/Using_IndexedDB). You can use it to persistently store data inside a user's browser, without having to rely on a backend database. It is dependency free. 

In technical terms Blazor ClientStorage does the following:

- Provides a client side library for communicating between C# and the browser
- Provides .NET classes to interact with the client side library
- Uses dependency injection for easy use and extensibility 


## What it isn't

Make sure you make yourself familiar with [IndexedDB](https://developer.mozilla.org/en-US/docs/Web/API/IndexedDB_API/Using_IndexedDB). The aim of this project is to expose IndexedDB to Blazor WebAssembly, nothing more.

- It is not a database
- It is not an ORM

## Installation

- Create a new Blazor WebAssembly app or use an existing one
- Install the latest version of the `Blazor.ClientStorage` package, [here](https://www.nuget.org/packages/Blazor.ClientStorage/)
- Add this dependency registrations to your `WebAssemblyHostBuilder`
  - `.AddBlazorClientStorage()`
- Add the following script tag to your `wwwroot\index.html`
  - `<script src="Blazor.ClientStorage.js"></script>`

# Usage

To use Blazor ClientStorage you need to use these concepts. 

- ObjectStoreModel<>
- ObjectStore

Create them like this
```csharp
public class Person : IObjectStoreModel<int>
{
    public int key { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }
}
```
```csharp
public class PersonObjectStore : AutoIncrementObjectStore<Person>
{
    public PersonObjectStore(IBlazorClientStorage blazorClientStorage) 
        : base(blazorClientStorage)
    {
    }

    public override string Name => nameof(Person);

    public override ObjectStoreDescriptor GetObjectStoreDescriptor()
    {
        return new()
        {
            Name = nameof(Person),
            Options = new()
            {
                autoIncrement = true,
            },
        };
    }
}
```

## Registration

Use `.AddObjectStore<>()` to register your ObjectStores for dependency injection.
```csharp
.AddObjectStore<PersonObjectStore>()
```

## Injection

Open `index.razor` (or any other Razor page) and inject your ObjectStore. 
```csharp
@inject PersonObjectStore personObjectStore
@code{ 
    protected async override Task OnInitializedAsync()
    {
        await personObjectStore.Add(new Person() {
            name = "Blazor",
            age = 23,
        });
    }
}
```

That's it. You should now have a new IndexedDB for your App setup. You can use `DevTools` in your browser to inspect the results. 

## Samples
https://github.com/Ericvf/Blazor.ClientStorage.Samples
