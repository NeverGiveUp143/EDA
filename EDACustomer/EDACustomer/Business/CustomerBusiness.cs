﻿using EDACustomer.Business.Interface;
using EDACustomer.Models;
using EDACustomer.Repository.Interface;
using EDADBContext.Models;
using Helper.Models;

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
            catch (Exception ex)
            {
                return new List<CustomerModel>();
            }
        }

        public async Task<string> AddCustomer(CustomerModel customer)
        {
            List<ModelMapping> customerModelMappingsValue = _configBusiness.GetMappingModel<ModelMapping>(Constants.CustomerAddModelMapping);
            Customer customerDataObj = Helper.ModelMapper.SourceModelToTargetModel<CustomerModel, Customer>(customer, customerModelMappingsValue);

            return await _customerRepository.AddCustomer(customerDataObj);
        }
    }
}
