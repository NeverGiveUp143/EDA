using EDACustomer.Business.Interface;
using EDACustomer.Models;
using EDACustomer.Repository.Interface;
using EDACustomer.Services.Interface;
using Microsoft.AspNetCore.SignalR;
using SignalR;

namespace EDACustomer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IProductBusiness _productBusiness;
        public CustomerService(IHubContext<NotificationHub> hubContext, IProductBusiness productBusiness)
        {
            _hubContext = hubContext;
            _productBusiness = productBusiness;
        }

        private async Task<int> GetLatestQuantityUpdate(Guid productId)
        {
            int quantity = 0;
            ProductModel product = await _productBusiness.GetProductById(productId);
            if (product != null)
            {
                quantity = product.Quantity;
            }
            return quantity;
        }

        public async Task<ProductModel> UpdatedProduct(Guid productId, int checkedOutQuantity)
        {
            ProductModel product = await _productBusiness.GetProductById(productId);
            if (product != null)
            {
                product.Quantity -= checkedOutQuantity;
            }
            return product ?? new();
        }


        public async Task UpdateStockAndNotify(Guid productId, int checkedOutQuantity)
        {
            int newQuantity = await GetLatestQuantityUpdate(productId);
            newQuantity -= checkedOutQuantity;

            await _hubContext.Clients.Group("InventoryUpdate")
               .SendAsync("ReceiveUpdate", "InventoryUpdate", new { ProductId = productId, Stock = newQuantity });
        }
    }
}
