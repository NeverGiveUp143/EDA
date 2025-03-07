namespace EDAInventory.Repository.Interface
{
    public interface IConfigRepository
    {
        T? GetConfigValue<T>(string Key);
    }
}
