#nullable disable
using System.ComponentModel.DataAnnotations;

namespace EDADBContext.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
    }
}
