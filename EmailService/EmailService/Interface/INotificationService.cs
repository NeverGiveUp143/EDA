namespace EmailService.Interface
{
    public interface INotificationService
    {
        Task SendEmailAsync<T>(T data, string mailBodyKey, string mailSubjectKey, string mailBodyMappingKey);
    }
}
