using EDACustomer.Business.Interface;
using EDACustomer.Models;
using EDACustomer.Repository.Interface;


namespace EDACustomer.Business
{
    public class ProductBusiness : IProductBusiness
    {
        private readonly IProductRepository _productRepository;
        public ProductBusiness(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductModel>> GetProductsList()
        {
            try
            {
                return await _productRepository.GetProductsList();
            }
            catch (Exception ex)
            {
                return new List<ProductModel>();
            }
        }
    }
}
