using Microsoft.AspNetCore.SignalR;
using PatteDoie.Rows.Scattegories;

namespace PatteDoie.Hubs
{
    public class ScattergoriesHub : Hub
    {
        public async Task SendProgression(Guid gameId, ScattergoriesPlayerRow player)
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

        public async Task ShowRanking(Guid gameId)
        {
            await Clients.Group(gameId.ToString()).SendAsync("ShowRanking");
        }

        public async Task SendWords(Guid gameId)
        {
            await Clients.Group(gameId.ToString()).SendAsync("SendWords");
        }
    }
}
