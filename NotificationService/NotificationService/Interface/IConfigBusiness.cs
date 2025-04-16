namespace EmailService.Interface
{
    public interface IConfigBusiness
    {
        T? GetConfigValue<T>(string Key);
    }
}
