namespace OrderService.Business.Interface
{
    public interface IConfigBusiness
    {
        T? GetConfigValue<T>(string Key);

        List<T> GetMappingModel<T>(string Key);
    }
}
