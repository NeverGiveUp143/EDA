﻿using EDACustomer.Business.Interface;
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

            if (string.IsNullOrEmpty(result))
            {
                Guid.TryParse(customer.Product, out Guid productId);
                var message = JsonConvert.SerializeObject(await _customerService.UpdatedProduct(productId, customer.ItemInCart)); 
                _rabbitMqPublisher.PublishMessage(message, "order_placed_queue");
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
