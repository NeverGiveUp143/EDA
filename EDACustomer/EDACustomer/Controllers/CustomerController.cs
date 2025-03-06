using EDACustomer.Business.Interface;
using EDACustomer.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDACustomer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerBusiness _customerBusiness;
        private readonly IProductBusiness _productBusiness;
        public CustomerController(ICustomerBusiness customerBusiness, IProductBusiness productBusiness)
        {
            _customerBusiness = customerBusiness;
            _productBusiness = productBusiness;
        }

        [HttpPost("AddCustomer")]
        public async Task<string> AddCustomer(CustomerModel customer)
        {
            return await _customerBusiness.AddCustomer(customer);
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

    }
}
