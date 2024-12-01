using Microsoft.AspNetCore.SignalR;

namespace PatteDoie.Hubs
{
    public class ScattergoriesHub : Hub
    {
        public Task JoinGame(Guid gameId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }

        public Task LeaveGame(Guid gameId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId.ToString());
        }
    }
}
