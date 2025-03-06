using EDACustomer.Business.Interface;
using EDACustomer.Repository.Interface;

namespace EDACustomer.Business
{

    public class ConfigBusiness : IConfigBusiness
    {

        private readonly IConfigRepository _configRepository;

        public ConfigBusiness(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        
        public T? GetConfigValue<T>(string Key)
        {
            return _configRepository.GetConfigValue<T>(Key);
        }
    }
}
