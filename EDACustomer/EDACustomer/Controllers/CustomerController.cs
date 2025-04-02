using EDACustomer.Business.Interface;
using EDACustomer.Models;
using EDACustomer.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQPublisher.Interface;

namespace EDACustomer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerBusiness _customerBusiness;
        private readonly IProductBusiness _productBusiness;
        private readonly ICustomerService _customerService;
        private readonly IRabbitMqPublisher _rabbitMqPublisher;

        public CustomerController(ICustomerBusiness customerBusiness, IProductBusiness productBusiness,
            ICustomerService customerService, IRabbitMqPublisher rabbitMqPublisher)
        {
            _customerBusiness = customerBusiness;
            _productBusiness = productBusiness;
            _customerService = customerService;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        [HttpPost("AddCustomer")]
        public async Task<string> AddCustomer(CustomerModel customer)
        {
            string result = await _customerBusiness.AddCustomer(customer);
            var message = string.Empty;

            if (string.IsNullOrEmpty(result))
            {
                message = JsonConvert.SerializeObject(new { eventType = "order.sucess", data = JsonConvert.SerializeObject(customer) });
                await _rabbitMqPublisher.PublishMessageAsync(message, "order_exchange", "order.sucess");
            }
            else
            {
                message = JsonConvert.SerializeObject(new { eventType = "order.failed", data = JsonConvert.SerializeObject(customer) });
                await _rabbitMqPublisher.PublishMessageAsync(message, "order_exchange", "order.failed");
            }

            return result;
        }


        [HttpGet("GetCustomers")]
        public async Task<ActionResult<List<CustomerModel>>> GetCustomers()
        {
            return await _customerBusiness.GetCustomersList();
        }

        [HttpGet("GetProducts")]
        public async Task<ActionResult<List<ProductModel>>> GetProducts()
        {
            return await _productBusiness.GetProductsList();
        }

        [HttpGet("GetProductDDList")]
        public async Task<ActionResult<List<ProductDDModel>>> GetProductDDList()
        {
            return await _productBusiness.GetProductDDList();
        }
    }
}
