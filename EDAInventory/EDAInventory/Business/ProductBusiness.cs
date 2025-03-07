using EDADBContext.Models;
using EDAInventory.Business.Interface;
using EDAInventory.Models;
using EDAInventory.Repository.Interface;
using Helper.Models;

namespace EDAInventory.Business
{
    public class ProductBusiness : IProductBusiness
    {
        private readonly IProductRepository _productRepository;
        private readonly IConfigBusiness _configBusiness;
        public ProductBusiness(IProductRepository productRepository , IConfigBusiness configBusiness)  
        {
            _productRepository = productRepository;
            _configBusiness = configBusiness;
        }
        public async Task<List<ProductModel>> GetProductsList()
        {
            try
            {
                List<ModelMapping> productModelMappingsValue = _configBusiness.GetMappingModel<ModelMapping>(Constants.ProductModelMapping);
                List<Product> DbValue = await _productRepository.GetProductsList();
                List<ProductModel> products = Helper.ModelMapper.SourceModelToTargetModel<Product, ProductModel>(DbValue, productModelMappingsValue);

                return products;
            }
            catch (Exception ex)
            {
                return new List<ProductModel>();
            }
        }

        public async Task<string> UpsertProduct(ProductModel product, bool IsUpdate = false)
        {
            string result = string.Empty;
            try
            {
                List<ModelMapping> productModelMappingsValue = _configBusiness.GetMappingModel<ModelMapping>(Constants.ProductModelMapping);
                Product productDBObj = Helper.ModelMapper.SourceModelToTargetModel<ProductModel, Product>(product, productModelMappingsValue);
                result = await _productRepository.UpsertProduct(productDBObj, IsUpdate);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        
    }
}
