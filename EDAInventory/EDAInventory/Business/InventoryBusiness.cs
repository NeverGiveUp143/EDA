using EDADBContext.Models;
using EDAInventory.Business.Interface;
using EDAInventory.Models;
using EDAInventory.Repository.Interface;
using Helper.Models;

namespace EDAInventory.Business
{
    public class InventoryBusiness : IInventoryBusiness
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IConfigBusiness _configBusiness;
        public InventoryBusiness(IInventoryRepository inventoryRepository , IConfigBusiness configBusiness)  
        {
            _inventoryRepository = inventoryRepository;
            _configBusiness = configBusiness;
        }
        public async Task<List<ProductModel>> GetProductsList()
        {
            try
            {
                List<ModelMapping> productModelMappingsValue = _configBusiness.GetMappingModel<ModelMapping>(Constants.ProductModelMapping);
                List<Product> DbValue = await _inventoryRepository.GetProductsList();
                List<ProductModel> products = Helper.ModelMapper.SourceModelToTargetModel<Product, ProductModel>(DbValue, productModelMappingsValue);

                return products;
            }
            catch (Exception ex)
            {
                return new List<ProductModel>();
            }
        }

        public async Task<string> UpsertProduct(ProductModel? product, bool IsUpdate = false)
        {
            string result = string.Empty;
            try
            {
                List<ModelMapping> productModelMappingsValue = _configBusiness.GetMappingModel<ModelMapping>(Constants.ProductModelMapping);
                Product productDBObj = Helper.ModelMapper.SourceModelToTargetModel<ProductModel, Product>(product, productModelMappingsValue);
                result = await _inventoryRepository.UpsertProduct(productDBObj, IsUpdate);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public async Task<string> ModifyStock(Guid productId, int quantity)
        {           
            var product = await _inventoryRepository.GetProductById(productId);
            if (product == null)
            {
                return $"Product {productId} not found in inventory.";
            }

            product.Quantity -= quantity;
            await _inventoryRepository.UpsertProduct(product,true);

           return string.Empty;
        }

        public async Task<string> GetProductById(Guid productId) 
        { 
            var product =  await _inventoryRepository.GetProductById(productId);
            if (product != null)
            {
                return product.Name;
            }
            return string.Empty;
        }
        
    }
}
