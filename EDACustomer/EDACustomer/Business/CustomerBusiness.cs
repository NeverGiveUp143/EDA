using APIHandler.Models;
using APIHandler;
using EDACustomer.Business.Interface;
using EDACustomer.Models;
using EDACustomer.Repository.Interface;
using EDADBContext.Models;
using Helper.Models;
using Newtonsoft.Json;

namespace EDACustomer.Business
{
    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IConfigBusiness _configBusiness;
        public CustomerBusiness(ICustomerRepository customerRepository, IConfigBusiness configBusiness)
        {
            _customerRepository = customerRepository;
            _configBusiness = configBusiness;
        }

        public async Task<List<CustomerModel>> GetCustomersList()
        {
            try
            {
                return await _customerRepository.GetCustomersList();
            }
            catch (Exception)
            {
                return new List<CustomerModel>();
            }
        }

        public async Task<string> AddCustomer(CustomerModel customer)
        {
            var requestPayload = JsonConvert.SerializeObject(customer);
            string? url = Environment.GetEnvironmentVariable(Constants.PaymentEndPoint) ?? "https://localhost:7253/Payment";
            APIResult<CustomerModel> response = await APIHandlerService<CustomerModel>.PostHandlerService(JsonConvert.SerializeObject(requestPayload), url);
            if (response != null)
            {
                List<ModelMapping> customerModelMappingsValue = _configBusiness.GetMappingModel<ModelMapping>(Constants.CustomerAddModelMapping);
                Customer customerDataObj = Helper.ModelMapper.SourceModelToTargetModel<CustomerModel, Customer>(customer, customerModelMappingsValue);

                return await _customerRepository.AddCustomer(customerDataObj);
            }
            else
            {
                return response?.Message ?? Constants.PaymentServiceFailed;
            }
        }
    }
}
