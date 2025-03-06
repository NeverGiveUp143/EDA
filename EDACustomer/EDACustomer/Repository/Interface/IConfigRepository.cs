namespace EDACustomer.Repository.Interface
{
    public interface IConfigRepository
    {
        T? GetConfigValue<T>(string Key);
    }
}
