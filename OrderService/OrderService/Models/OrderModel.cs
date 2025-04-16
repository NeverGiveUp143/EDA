#nullable disable
namespace OrderService.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Product { get; set; }
        public string ProductName { get; set; }
        public int ItemInCart { get; set; }
        public string Email { get; set; }
    }
}
