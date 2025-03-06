namespace EDACustomer.Business.Interface
{
    public interface IConfigBusiness
    {
        T? GetConfigValue<T>(string Key);
    }
}
