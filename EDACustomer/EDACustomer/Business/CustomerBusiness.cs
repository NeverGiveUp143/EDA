using EDACustomer.Business.Interface;
using EDACustomer.Models;
using EDACustomer.Repository.Interface;
using EDADBContext.Models;
using Helper.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace EDACustomer.Business
{
    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IConfigRepository _configRepository;
        public CustomerBusiness(ICustomerRepository customerRepository, IConfigRepository configRepository)
        {
            _customerRepository = customerRepository;
            _configRepository = configRepository;
        }

        public async Task<List<CustomerModel>> GetCustomersList()
        {
            try
            {
                string? configValue = _configRepository.GetConfigValue<string>(Constants.CustomerModelMapping);
                List<ModelMapping> customerModelMappingsValue = !string.IsNullOrEmpty(configValue) ? JsonConvert.DeserializeObject<List<ModelMapping>>(configValue) ?? [] : [];
                List<Customer> DbValue = await _customerRepository.GetCustomersList();
                List<CustomerModel> customers = Helper.ModelMapper.SourceModelToTargetModel<Customer, CustomerModel>(DbValue, customerModelMappingsValue);
                return customers;
            }
            catch (Exception ex)
            {
                return new List<CustomerModel>();
            }
        }

        public async Task<string> AddCustomer(CustomerModel customer)
        {
            string? configValue = _configRepository.GetConfigValue<string>(Constants.CustomerModelMapping);
            List<ModelMapping> customerModelMappingsValue = !string.IsNullOrEmpty(configValue) ? JsonConvert.DeserializeObject<List<ModelMapping>>(configValue) ?? [] : [];
            Customer customerDataObj = Helper.ModelMapper.SourceModelToTargetModel<CustomerModel, Customer>(customer, customerModelMappingsValue);

            return await _customerRepository.AddCustomer(customerDataObj);
        }
    }
}
