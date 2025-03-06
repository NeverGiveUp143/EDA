using EDADBContext.Models;
using EDAInventory.Business.Interface;
using EDAInventory.Repository.Interface;

namespace EDAInventory.Business
{
    public class ProductBusiness : IProductBusiness
    {
        private readonly IProductRepository _productRepository;
        public ProductBusiness(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<List<Product>> GetProductsList()
        {
            try
            {
                return await _productRepository.GetProductsList();
            }
            catch (Exception ex)
            {
                return new List<Product>();
            }
        }

    }
}
