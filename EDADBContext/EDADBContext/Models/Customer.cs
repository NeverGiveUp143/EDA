#nullable disable
using System.ComponentModel.DataAnnotations;

namespace EDADBContext.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid ProductId { get; set; }
        public int ItemInCart { get; set; }
    }
}
