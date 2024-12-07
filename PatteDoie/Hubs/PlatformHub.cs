using Microsoft.AspNetCore.SignalR;

namespace PatteDoie.Hubs
{
    public class PlatformHub : Hub
    {
        public async Task SendGameStarted(Guid lobbyId, Guid gameId)
        {
            await Clients.Group(lobbyId.ToString()).SendAsync("ReceiveGameStarted", gameId);
        }

        public Task JoinLobby(Guid lobbyId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, lobbyId.ToString());
        }

        public Task LeaveLobby(Guid lobbyId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyId.ToString());
        }
    }
}
