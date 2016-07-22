namespace Web.Hub
{
    using Microsoft.AspNet.SignalR;

    public class FoxyHub : Hub
    {
        public void PushStatus(string newStatus)
        {
            Clients.All.newParserStatus(newStatus);
        }
    }
}