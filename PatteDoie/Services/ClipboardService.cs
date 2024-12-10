using Microsoft.JSInterop;

namespace PatteDoie.Services
{
    public class ClipboardService(IJSRuntime JsRuntTime) : IClipboardService
    {
        private readonly IJSRuntime JsRuntTime = JsRuntTime;

        public async Task CopyLobbyLink(Guid lobbyId)
        {
            await JsRuntTime.InvokeAsync<string>("navigator.clipboard.writeText", "https://localhost:8081/lobby/join/" + lobbyId);
        }
    }
}
