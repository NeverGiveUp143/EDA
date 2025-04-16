using EmailService.Interface;
using Newtonsoft.Json;

namespace EmailService
{
    public class ConfigBusiness : IConfigBusiness
    {
        private readonly IConfigRepository _configRepository;

        public ConfigBusiness(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public T? GetConfigValue<T>(string key)
        {
            return _configRepository.GetConfigValue<T>(key);
        }

    }

}
