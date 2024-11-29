using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PatteDoie.Services.Platform;

namespace PatteDoie.Views.Platform
{
    public partial class LobbyJoin : AuthenticatedPage
    {
        [BindProperty(SupportsGet = true)]
        [Parameter]
        public required string Id { get; set; }
        private string? Password { get; set; }

        [Inject]
        private IPlatformService PlatformService { get; set; } = default!;

        private async void Submit()
        {
            var uuid = await GetUUID();
            var name = await GetName();

            await PlatformService.JoinLobby(new Guid(Id ?? ""), name, new Guid(uuid), Password);

            NavigationManager.NavigateTo($"/lobby/{Id}");
        }

    }
}
