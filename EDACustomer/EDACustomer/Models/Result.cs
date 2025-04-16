#nullable disable
namespace OrderService.Models
{
    public class Result<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool IsSucess { get; set; }
        public int HttpStatusCode { get; set; }
    }
}
