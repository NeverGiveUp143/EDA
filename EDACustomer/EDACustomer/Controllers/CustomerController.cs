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
            Guid.TryParse(customer.Product, out Guid productId);
            customer.ProductName = await _productBusiness.GetProductNameById(productId);
            var message = string.Empty;
            int.TryParse(result, out int custID);

            if (custID > 0)
            {
                customer.Id = custID;
                message = JsonConvert.SerializeObject(new { eventType = "payment.sucess", data = JsonConvert.SerializeObject(customer) });
                await _rabbitMqPublisher.PublishMessageAsync(message, "payment_status", "payment.sucess");

                return string.Empty;
            }
            else
            {
                message = JsonConvert.SerializeObject(new { eventType = "payment.failed", data = JsonConvert.SerializeObject(customer) });
                await _rabbitMqPublisher.PublishMessageAsync(message, "payment_status", "payment.failed");
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
