namespace SignalR
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class NotificationHub : Hub
    {
        public async Task SendUpdate(string eventType, object payload)
        {
            await Clients.Group(eventType).SendAsync("ReceiveUpdate", eventType, payload);
        }

        public async Task SubscribeToEvent(string eventType)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, eventType);
        }

        public async Task UnsubscribeFromEvent(string eventType)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, eventType);
        }
    }

}
