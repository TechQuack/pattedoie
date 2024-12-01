using Microsoft.AspNetCore.SignalR;
using PatteDoie.Rows.SpeedTyping;

namespace PatteDoie.Hubs
{
    public class SpeedTypingHub : Hub
    {
        public async Task SendProgression(Guid gameId, SpeedTypingPlayerRow player)
        {
            await Clients.Group(gameId.ToString()).SendAsync("ReceiveProgression", player);
        }

        public Task JoinGame(Guid gameId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }

        public Task LeaveGame(Guid gameId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId.ToString());
        }

        public async Task RedirectToHome(Guid gameId)
        {
            await Clients.Group(gameId.ToString()).SendAsync("RedirectToHome");
        }
    }
}
