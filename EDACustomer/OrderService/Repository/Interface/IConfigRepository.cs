namespace OrderService.Repository.Interface
{
    public interface IConfigRepository
    {
        T? GetConfigValue<T>(string Key);
    }
}
