﻿using OrderService.Business.Interface;
using OrderService.Repository.Interface;
using Newtonsoft.Json;

namespace OrderService.Business
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

        public List<T> GetMappingModel<T>(string key)
        {
            string? configValue = _configRepository.GetConfigValue<string>(key);
            return  !string.IsNullOrWhiteSpace(configValue) ? JsonConvert.DeserializeObject<List<T>>(configValue) ?? [] : [];
        }
    }
}
