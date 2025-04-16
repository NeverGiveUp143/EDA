using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderService.Business.Interface;
using OrderService.Models;
using RabbitMQ.Client;
using RabbitMQPublisher.Interface;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderBusiness _orderBusiness;
        private readonly IProductBusiness _productBusiness;
        private readonly IRabbitMqPublisher _rabbitMqPublisher;

        public OrderController(IOrderBusiness orderBusiness, IProductBusiness productBusiness, IRabbitMqPublisher rabbitMqPublisher)
        {
            _orderBusiness = orderBusiness;
            _productBusiness = productBusiness;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        [HttpPost("AddCheckoutOrder")]
        public async Task<string> AddCheckoutOrder(OrderModel order)
        {
            string result = await _orderBusiness.AddCheckoutOrder(order);
            Guid.TryParse(order.Product, out Guid productId);
            order.ProductName = await _productBusiness.GetProductNameById(productId);
            var message = string.Empty;
            int.TryParse(result, out int orderID);

            if (orderID > 0)
            {
                order.Id = orderID;
                message = JsonConvert.SerializeObject(new { eventType = "payment.sucess", data = JsonConvert.SerializeObject(order) });
                await _rabbitMqPublisher.PublishMessageAsync(message, "payment_exchange", "payment.sucess", "payment_queue", ExchangeType.Fanout);

                return string.Empty;
            }
            else
            {
                message = JsonConvert.SerializeObject(new { eventType = "payment.failed", data = JsonConvert.SerializeObject(order) });
                await _rabbitMqPublisher.PublishMessageAsync(message, "payment_exchange", "payment.failed", "payment_queue", ExchangeType.Fanout);
            }

            return result;
        }


        [HttpGet("GetOrdersCheckoutList")]
        public async Task<ActionResult<List<OrderModel>>> GetOrdersCheckoutList()
        {
            return await _orderBusiness.GetOrdersCheckOutList();
        }
      
        [HttpGet("GetProductDDList")]
        public async Task<ActionResult<List<ProductDDModel>>> GetProductDDList()
        {
            return await _productBusiness.GetProductDDList();
        }
    }
}
