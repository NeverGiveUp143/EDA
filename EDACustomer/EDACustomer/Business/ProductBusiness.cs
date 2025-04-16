using OrderService.Business.Interface;
using OrderService.Models;
using OrderService.Repository.Interface;
using EDADBContext.Models;
using Helper.Models;


namespace OrderService.Business
{
    public class ProductBusiness : IProductBusiness
    {
        private readonly IProductRepository _productRepository;
        private readonly IConfigBusiness _configBusiness;
        public ProductBusiness(IProductRepository productRepository, IConfigBusiness configBusiness)
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
                return new ();
            }
        }

        public async Task<ProductModel> GetProductById(Guid productId)
        {
            try
            {
                List<ModelMapping> productModelMappingsValue = _configBusiness.GetMappingModel<ModelMapping>(Constants.ProductModelMapping);
                Product? DbValue = await _productRepository.GetProductById(productId);
                ProductModel product = Helper.ModelMapper.SourceModelToTargetModel<Product, ProductModel>(DbValue, productModelMappingsValue);

                return product;
            }
            catch (Exception ex)
            {
                return new ();
            }
        }

        public async Task<List<ProductDDModel>> GetProductDDList()
        {
            try
            {
                return await _productRepository.GetProductDDList();  
            }
            catch (Exception ex)
            {
                return new();
            }
        }

        public async Task<string> GetProductNameById(Guid productId)
        {

            var product = await _productRepository.GetProductNameById(productId);
            if (product != null)
            {
                return product.Name;
            }
            return string.Empty;
        }

    }
}
