#nullable disable
namespace EDACustomer.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Product { get; set; }
        public string ProductName { get; set; }
        public int ItemInCart { get; set; }
        public string Email { get; set; }
    }
}
