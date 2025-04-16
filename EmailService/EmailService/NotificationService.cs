using EmailService.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EmailService
{
    public class NotificationService : INotificationService
    {

        private readonly IConfigBusiness _configBusiness;

        public NotificationService(IConfigBusiness configBusiness)
        {
            _configBusiness = configBusiness;
        }

        public async Task SendEmailAsync<T>(T data, string mailBodyKey, string mailSubjectKey, string mailBodyMappingKey)
        {
            // Extract values from config
            string toName = data?.GetType().GetProperty("Name")?.GetValue(data)?.ToString() ?? "";
            string toEmail = data?.GetType().GetProperty("Email")?.GetValue(data)?.ToString() ?? "";
            string mailSubject = _configBusiness.GetConfigValue<string>(mailSubjectKey) ?? "";

            string mailBody = _configBusiness.GetConfigValue<string>(mailBodyKey) ?? string.Empty;
            List<string> mailBodyMapping = _configBusiness
                .GetConfigValue<string>(mailBodyMappingKey)?
                .Split(',')
                .Select(s => s.Trim())
                .ToList() ?? new List<string>();

            List<object> mappedValues = new();
            foreach (var key in mailBodyMapping)
            {
                var value = data?.GetType().GetProperty(key)?.GetValue(data);
                mappedValues.Add(value ?? "");
            }

            // Format the mail body
            string formattedBody = string.Format(mailBody, mappedValues.ToArray());

            // Prepare and send the email
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(Constants.FromName, Constants.FromEmail));
            email.To.Add(new MailboxAddress(toName, toEmail));
            email.Subject = mailSubject;

            email.Body = new TextPart("html") { Text = formattedBody };

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(Constants.FromEmail, Constants.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email failed: {ex.Message}");
            }
        }


    }
}
