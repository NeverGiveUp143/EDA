using APIHandler.Models;
using APIHandler;
using OrderService.Business.Interface;
using OrderService.Models;
using OrderService.Repository.Interface;
using EDADBContext.Models;
using Helper.Models;
using Newtonsoft.Json;

namespace OrderService.Business
{
    public class OrderBusiness : IOrderBusiness
    {
        private readonly IOrderRepository _customerRepository;
        private readonly IConfigBusiness _configBusiness;
        public OrderBusiness(IOrderRepository customerRepository, IConfigBusiness configBusiness)
        {
            _customerRepository = customerRepository;
            _configBusiness = configBusiness;
        }

        public async Task<List<OrderModel>> GetOrdersCheckOutList()
        {
            try
            {
                return await _customerRepository.GetOrdersCheckOutList();
            }
            catch (Exception)
            {
                return new List<OrderModel>();
            }
        }

        public async Task<string> AddCheckoutOrder(OrderModel customer)
        {
            var requestPayload = JsonConvert.SerializeObject(customer);
            string? url = Environment.GetEnvironmentVariable(Constants.PaymentEndPoint) ?? "https://localhost:7253/Payment";
            APIResult<OrderModel> response = await APIHandlerService<OrderModel>.PostHandlerService(JsonConvert.SerializeObject(requestPayload), url);
            if (response != null)
            {
                List<ModelMapping> customerModelMappingsValue = _configBusiness.GetMappingModel<ModelMapping>(Constants.CustomerAddModelMapping);
                Customer customerDataObj = Helper.ModelMapper.SourceModelToTargetModel<OrderModel, Customer>(customer, customerModelMappingsValue);

                return await _customerRepository.AddCheckoutOrder(customerDataObj);
            }
            else
            {
                return response?.Message ?? Constants.PaymentServiceFailed;
            }
        }
    }
}
