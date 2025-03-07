using EDAInventory.Business.Interface;
using EDAInventory.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDAInventory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IProductBusiness _productBusiness;
        public InventoryController(IProductBusiness productBusiness)
        {
            _productBusiness = productBusiness;
        }

        [HttpGet("GetProducts")]
        public async Task<ActionResult<List<ProductModel>>> GetProducts()
        {
            return await _productBusiness.GetProductsList();
        }

        
    }
}
