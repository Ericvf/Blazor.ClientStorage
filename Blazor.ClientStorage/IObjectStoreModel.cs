namespace Blazor.ClientStorage
{
    public interface IObjectStoreModel<TKey>
    {
        public TKey key { get; set; }
    }
}
