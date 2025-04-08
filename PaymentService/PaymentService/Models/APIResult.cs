#nullable disable
namespace APIHandler.Models
{
    /// <summary>
    /// API Result 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIResult<T>
    {
        public T value { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public int HttpStatus { get; set; }
    }
}
