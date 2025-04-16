namespace EmailService.Interface
{
    public interface IConfigRepository
    {
        T? GetConfigValue<T>(string Key);
    }
}
