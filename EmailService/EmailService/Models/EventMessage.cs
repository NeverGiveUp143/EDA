#nullable disable
namespace EmailService.Models
{
    public class EventMessage<T>
    {
        public string EventType { get; set; }
        public T Data { get; set; }
    }
}
