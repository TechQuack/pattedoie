using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using PatteDoie.Services.Platform;

namespace PatteDoie.Views.Platform
{
    public partial class LobbyJoin : ComponentBase
    {
        [BindProperty(SupportsGet = true)]
        [Parameter]
        public required string Id { get; set; }

        private string? Password { get; set; }

        [Inject]
        private ProtectedLocalStorage ProtectedLocalStorage { get; set; } = default!;

        [Inject]
        private IPlatformService PlatformService { get; set; } = default!;

        private async void Submit()
        {
            var uuid = await ProtectedLocalStorage.GetAsync<string>("uuid");
            var name = await ProtectedLocalStorage.GetAsync<string>("name");

            try
            {
                await PlatformService.JoinLobby(new Guid(Id ?? ""), name.Value ?? "", new Guid(uuid.Value ?? ""), Password);
            }
            catch (Exception ex)
            {
                // TODO : display an error to the user
            }
        }

    }
}
